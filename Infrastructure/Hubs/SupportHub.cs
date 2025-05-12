namespace Sufra_MVC.Infrastructure.Hubs
{
    using Microsoft.AspNetCore.SignalR;

    public class SupportHub : Hub
    {
        public async Task SendSupportMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveSupportMessage", user, message);
        }
    }

}
