using System.Text;
using System.Text.Json;

using RTN.API.Shared.Models;

namespace RTN.API.Services;

public class NotificationService(HttpClient httpClient) {
    public async Task SendNotificationAsync(NotificationModel message, Guid? roomId = null) {
        SocketNotificationMessage body = new(message, roomId);

        var jsonContent = JsonSerializer.Serialize(body);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync("/send-notification", content);
        response.EnsureSuccessStatusCode();
    }
}
