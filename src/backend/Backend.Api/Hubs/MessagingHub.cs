using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Backend.Api.Hubs
{
    [Authorize]
    public class MessagingHub : Hub
    {
        public async Task NewMessage(string message)
        {
            var internalMessage = $"{message} @ ${DateTime.UtcNow}";
            await Clients.All.SendAsync("messaging", internalMessage);
        }
    }
}
