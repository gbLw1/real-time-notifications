using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RTN.API.Data;
using RTN.API.Data.Entities;
using RTN.API.Shared.Args;
using RTN.API.Shared.Models;
using static RTN.API.Shared.Extensions.HttpRequestExtensions;

namespace RTN.API.Controllers;

/// <summary>
/// The admin notifications controller.
/// This controller is public just for testing purposes
/// in a real-world scenario it should be private and only accessible by admins (role-based access control).
/// </summary>
[ApiController]
[Route("[controller]")]
public class AdminNotificationsController(
    ILogger<AdminNotificationsController> logger,
    MyDbContext dbContext)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            logger.LogInformation("Getting user notifications.");

            var notifications = await dbContext
                .Set<NotificationEntity>()
                .Select(n => new NotificationModel
                {
                    Id = n.Id,
                    Content = n.Content,
                    RedirectUrl = n.RedirectUrl,
                    IsRead = n.IsRead
                })
                .ToListAsync();

            return Ok(notifications);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to get notifications.");
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{notificationId:guid}")]
    public async Task<IActionResult> Get([FromRoute] Guid notificationId)
    {
        try
        {
            logger.LogInformation("Getting notification.");

            var notification = await dbContext.Set<NotificationEntity>()
                .Select(n => new NotificationModel
                {
                    Id = n.Id,
                    Content = n.Content,
                    RedirectUrl = n.RedirectUrl,
                    IsRead = n.IsRead
                })
                .FirstOrDefaultAsync(n => n.Id == notificationId)
                ?? throw new InvalidOperationException("Notification not found.");

            return Ok(notification);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to get notification.");
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> PostGlobal(
        [FromBody] NotificationPostArgs args)
    {
        try
        {
            logger.LogInformation("Creating notification.");

            new NotificationPostArgs.Validator().ValidateAndThrow(args);

            var notification = new NotificationEntity
            {
                Content = args.Content,
                RedirectUrl = args.RedirectUrl,
                IsRead = false,
            };

            dbContext.Set<NotificationEntity>().Add(notification);
            await dbContext.SaveChangesAsync();

            try
            {
                logger.LogInformation("Sending notification.");
                await SendNotificationAsync(
                    message: new NotificationModel
                    {
                        Id = notification.Id,
                        Content = notification.Content,
                        RedirectUrl = notification.RedirectUrl,
                        IsRead = notification.IsRead
                    });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to send notification.");
                return BadRequest(ex.Message);
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create notification.");
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{userId:guid}")]
    public async Task<IActionResult> PostIndividual(
        [FromRoute] Guid userId,
        [FromBody] NotificationPostArgs args)
    {
        try
        {
            logger.LogInformation("Creating notification.");

            new NotificationPostArgs.Validator().ValidateAndThrow(args);

            var notification = new NotificationEntity
            {
                Content = args.Content,
                RedirectUrl = args.RedirectUrl,
                IsRead = false,
                UserId = userId
            };

            dbContext.Set<NotificationEntity>().Add(notification);
            await dbContext.SaveChangesAsync();

            try
            {
                logger.LogInformation("Sending notification.");
                await SendNotificationAsync(
                    message: new NotificationModel
                    {
                        Id = notification.Id,
                        Content = notification.Content,
                        RedirectUrl = notification.RedirectUrl,
                        IsRead = notification.IsRead
                    },
                    roomId: userId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to send notification.");
                return BadRequest(ex.Message);
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create notification.");
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{Id:guid}")]
    public async Task<IActionResult> Put(
        [FromRoute] Guid Id,
        [FromBody] NotificationPutArgs args)
    {
        try
        {
            logger.LogInformation("Updating notification.");

            new NotificationPutArgs.Validator().ValidateAndThrow(args);

            var notification = await dbContext.Set<NotificationEntity>()
                .FirstOrDefaultAsync(n => n.Id == Id)
                ?? throw new InvalidOperationException("Notification not found.");

            notification.Content = args.Content;
            notification.RedirectUrl = args.RedirectUrl;
            notification.IsRead = args.IsRead;

            dbContext.Set<NotificationEntity>().Update(notification);
            await dbContext.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update notification.");
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{Id:guid}")]
    public async Task<IActionResult> Delete(Guid Id)
    {
        try
        {
            logger.LogInformation("Deleting notification.");

            var notification = await dbContext.Set<NotificationEntity>()
                .FirstOrDefaultAsync(n => n.Id == Id)
                ?? throw new InvalidOperationException("Notification not found.");

            dbContext.Set<NotificationEntity>().Remove(notification);
            await dbContext.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete notification.");
            return BadRequest(ex.Message);
        }
    }
}