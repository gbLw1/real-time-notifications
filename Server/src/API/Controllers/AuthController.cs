using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RTN.API.Data;
using RTN.API.Data.Entities;
using RTN.API.Shared.Args;
using RTN.API.Shared.Models;

namespace RTN.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(
    ILogger<AuthController> logger,
    MyDbContext dbContext)
    : ControllerBase
{
    [HttpPost("token")]
    public async Task<IActionResult> Login(AuthTokenArgs args)
    {
        try
        {
            logger.LogInformation("Logging in.");

            new AuthTokenArgs.Validator().ValidateAndThrow(args);

            var login = await dbContext.Set<LoginEntity>()
                .Include(l => l.User)
                .FirstOrDefaultAsync(l => l.User!.Email == args.Email);

            if (
                login is null ||
                !BCrypt.Net.BCrypt.Verify(args.Password, login.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            var authToken = Guid.NewGuid();
            var expiresIn = DateTime.UtcNow.AddHours(1);
            var refreshToken = Guid.NewGuid();

            login.AuthToken = authToken;
            login.AuthTokenExpiryTime = expiresIn;
            login.RefreshToken = refreshToken;
            login.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            dbContext.Update(login);
            await dbContext.SaveChangesAsync();

            return Ok(new AuthTokenModel
            {
                AuthToken = authToken,
                ExpiresIn = expiresIn,
                RefreshToken = refreshToken,
                SocketRoom = login.UserId,
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogError(ex, "Failed to login with 401.");
            return Unauthorized(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to login.");
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterArgs args)
    {
        try
        {
            logger.LogInformation("Registering.");

            new RegisterArgs.Validator().ValidateAndThrow(args);

            var user = await dbContext.Set<UserEntity>()
                .FirstOrDefaultAsync(u => u.Email == args.Email);

            if (user is not null)
            {
                throw new InvalidOperationException("Email is already in use.");
            }

            await dbContext.Database.BeginTransactionAsync();

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(args.Password);

            var userEntity = new UserEntity
            {
                Name = args.Name,
                Email = args.Email,
            };

            await dbContext.Set<UserEntity>().AddAsync(userEntity);
            await dbContext.SaveChangesAsync();

            var login = new LoginEntity
            {
                AuthToken = Guid.NewGuid(),
                PasswordHash = passwordHash,
                RefreshToken = Guid.NewGuid(),
                UserId = userEntity.Id,
                AuthTokenExpiryTime = DateTime.UtcNow.AddHours(1),
                RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7)
            };

            await dbContext.Set<LoginEntity>().AddAsync(login);
            await dbContext.SaveChangesAsync();
            await dbContext.Database.CommitTransactionAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to register.");
            if (dbContext.Database.CurrentTransaction is not null)
            {
                await dbContext.Database.RollbackTransactionAsync();
            }
            return BadRequest(ex.Message);
        }
    }

}
