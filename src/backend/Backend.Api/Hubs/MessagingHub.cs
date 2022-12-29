using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Backend.Api.Hubs
{
    [Authorize]
    public class MessagingHub : Hub { }
}
