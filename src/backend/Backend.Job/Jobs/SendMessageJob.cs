using Backend.Job.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Backend.Job.Jobs
{
    public class SendMessageJob
	{
        private readonly IHubContext<MessagingHub> _hub;

        public SendMessageJob(IHubContext<MessagingHub> hub)
		{
			_hub = hub;
		}

		public Task SendAsync(string message)
		{
			var internalMessage = $"{message} @ {DateTime.UtcNow}";
			return _hub.Clients.All.SendAsync("messaging", internalMessage);
		}
	}
}

