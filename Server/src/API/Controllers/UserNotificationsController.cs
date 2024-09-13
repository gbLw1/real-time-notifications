using FluentValidation;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using RTN.API.Data;
using RTN.API.Data.Entities;
using RTN.API.Shared.Extensions;
using RTN.API.Shared.Models;

namespace RTN.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserNotificationsController(
    ILogger<UserNotificationsController> logger,
    MyDbContext dbContext)
    : ControllerBase {
    [HttpGet]
    public async Task<IActionResult> Get() {
        try {
            logger.LogInformation("Getting user notifications.");

            var authToken = this.GetAuthToken()!;

            var notifications = await dbContext
                .Set<NotificationEntity>()
                .Where(n => n.UserId == authToken.UserId)
                .Select(n => new NotificationModel(n.Id, n.Content, n.RedirectUrl, n.IsRead))
                .ToListAsync();

            return Ok(notifications);
        }
        catch (UnauthorizedAccessException ex) {
            logger.LogError(ex, "Failed to get notifications with 401.");
            return Unauthorized(ex.Message);
        }
        catch (Exception ex) {
            logger.LogError(ex, "Failed to get notifications.");
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{notificationId:guid}")]
    public async Task<IActionResult> Get([FromRoute] Guid notificationId) {
        try {
            logger.LogInformation("Getting notification.");

            var authToken = this.GetAuthToken()!;

            var notification = await dbContext
                .Set<NotificationEntity>()
                .Select(n => new NotificationModel(n.Id, n.Content, n.RedirectUrl, n.IsRead))
                .FirstOrDefaultAsync(n => n.Id == notificationId)
                ?? throw new InvalidOperationException("Notification not found.");

            return Ok(notification);
        }
        catch (UnauthorizedAccessException ex) {
            logger.LogError(ex, "Failed to get notifications with 401.");
            return Unauthorized(ex.Message);
        }
        catch (Exception ex) {
            logger.LogError(ex, "Failed to get notification.");
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete]
    public async Task<IActionResult> Delete() {
        try {
            logger.LogInformation("Deleting all notifications.");

            var authToken = this.GetAuthToken()!;

            var notifications = await dbContext.Set<NotificationEntity>()
                .Where(n => n.UserId == authToken.UserId)
                .ToListAsync();

            dbContext.Set<NotificationEntity>().RemoveRange(notifications);
            await dbContext.SaveChangesAsync();

            return NoContent();
        }
        catch (UnauthorizedAccessException ex) {
            logger.LogError(ex, "Failed to delete notifications with 401.");
            return Unauthorized(ex.Message);
        }
        catch (Exception ex) {
            logger.LogError(ex, "Failed to delete notifications.");
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{Id:guid}")]
    public async Task<IActionResult> Delete(Guid Id) {
        try {
            logger.LogInformation("Deleting notification.");

            var authToken = this.GetAuthToken()!;

            var notification = await dbContext.Set<NotificationEntity>()
                .Where(n => n.UserId == authToken.UserId)
                .FirstOrDefaultAsync(n => n.Id == Id)
                ?? throw new InvalidOperationException("Notification not found.");

            dbContext.Set<NotificationEntity>().Remove(notification);
            await dbContext.SaveChangesAsync();

            return NoContent();
        }
        catch (UnauthorizedAccessException ex) {
            logger.LogError(ex, "Failed to delete notification with 401.");
            return Unauthorized(ex.Message);
        }
        catch (Exception ex) {
            logger.LogError(ex, "Failed to delete notification.");
            return BadRequest(ex.Message);
        }
    }

    [HttpPatch("{Id:guid}/toggle-read")]
    public async Task<IActionResult> ToggleRead(Guid Id) {
        try {
            logger.LogInformation("Toggling notification read status.");

            var authToken = this.GetAuthToken()!;

            var notification = await dbContext.Set<NotificationEntity>()
                .Where(n => n.UserId == authToken.UserId)
                .FirstOrDefaultAsync(n => n.Id == Id)
                ?? throw new InvalidOperationException("Notification not found.");

            notification.IsRead = !notification.IsRead;

            dbContext.Set<NotificationEntity>().Update(notification);
            await dbContext.SaveChangesAsync();

            return NoContent();
        }
        catch (UnauthorizedAccessException ex) {
            logger.LogError(ex, "Failed to toggle notification read status with 401.");
            return Unauthorized(ex.Message);
        }
        catch (Exception ex) {
            logger.LogError(ex, "Failed to toggle notification read status.");
            return BadRequest(ex.Message);
        }
    }
}
