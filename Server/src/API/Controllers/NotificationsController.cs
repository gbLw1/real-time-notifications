using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RTN.API.Data;
using RTN.API.Data.Entities;
using RTN.API.Shared.Args;
using RTN.API.Shared.Args.Validations;
using RTN.API.Shared.Extensions;
using RTN.API.Shared.Models;

namespace RTN.API.Controllers;

[ApiController]
[Route("[controller]")]
public class NotificationsController(
    ILogger<NotificationsController> logger,
    MyDbContext dbContext)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            logger.LogInformation("Getting user notifications.");

            var authToken = await Request.GetAuthTokenAsync(dbContext);

            var notifications = await dbContext
                .Set<NotificationEntity>()
                .Where(n => n.UserId == authToken.UserId)
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
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Unauthorized access.");
            return Unauthorized(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to get notifications.");
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{Id:guid}")]
    public async Task<IActionResult> Get(Guid Id)
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
                .FirstOrDefaultAsync(n => n.Id == Id)
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
    public async Task<IActionResult> Post([FromBody] NotificationPostArgs args)
    {
        try
        {
            logger.LogInformation("Creating notification.");

            new NotificationPostArgsValidator().ValidateAndThrow(args);

            var notification = new NotificationEntity
            {
                Content = args.Content,
                RedirectUrl = args.RedirectUrl,
                IsRead = false
            };

            dbContext.Set<NotificationEntity>().Add(notification);
            await dbContext.SaveChangesAsync();

            await SendNotificationAsync(args.Content);

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

            new NotificationPutArgsValidator().ValidateAndThrow(args);

            var notification = await dbContext.Set<NotificationEntity>()
                .FirstOrDefaultAsync(n => n.Id == Id)
                ?? throw new InvalidOperationException("Notification not found.");

            notification.Content = args.Content;
            notification.RedirectUrl = args.RedirectUrl;
            notification.IsRead = args.IsRead;

            dbContext.Set<NotificationEntity>().Update(notification);
            await dbContext.SaveChangesAsync();

            var result = new NotificationModel
            {
                Id = notification.Id,
                Content = notification.Content,
                RedirectUrl = notification.RedirectUrl,
                IsRead = notification.IsRead
            };

            return Ok(result);
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

    [HttpPatch("{Id:guid}/toggle-read")]
    public async Task<IActionResult> ToggleRead(Guid Id)
    {
        try
        {
            logger.LogInformation("Toggling notification read status.");

            var notification = await dbContext.Set<NotificationEntity>()
                .FirstOrDefaultAsync(n => n.Id == Id)
                ?? throw new InvalidOperationException("Notification not found.");

            notification.IsRead = !notification.IsRead;

            dbContext.Set<NotificationEntity>().Update(notification);
            await dbContext.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to toggle notification read status.");
            return BadRequest(ex.Message);
        }
    }

    public class NotificationMessage(string message, string? roomId)
    {
        [JsonPropertyName("message")]
        public string Message { get; set; } = message;
        [JsonPropertyName("roomId")]
        public string? RoomId { get; set; } = roomId;
    }

    public async Task SendNotificationAsync(string message, string? roomId = null)
    {
        try
        {
            NotificationMessage bodyObj = new(message, null);

            if (roomId is not null)
            {
                bodyObj.RoomId = roomId;
            }

            var jsonContent = JsonSerializer.Serialize(bodyObj);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            using var httpClient = new HttpClient();
            var response = await httpClient.PostAsync("http://localhost:3069/send-notification", content);

            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send notification.");
        }
    }
}