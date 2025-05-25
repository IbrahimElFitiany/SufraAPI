namespace SufraMVC.Infrastructure.Hubs
{
    using Microsoft.AspNetCore.SignalR;
    using System.Collections.Concurrent;

    public class ChatHub : Hub
    {
        private static ConcurrentDictionary<string, string> _userConnections = new();
        private static ConcurrentQueue<string> _availableSupportAgents = new();

        public Task RegisterUser()
        {
            var userId = Context.User?.FindFirst("UserID")?.Value;

            if (userId != null)
            {
                _userConnections[userId] = Context.ConnectionId;
                Console.WriteLine($"User {userId} registered with ConnectionId: {Context.ConnectionId}");
            }
            else
            {
                Console.WriteLine("UserID not found in token.");
            }

            return Task.CompletedTask;
        }

        public async Task RegisterSupport()
        {
            var supportId = Context.User?.FindFirst("UserID")?.Value;

            if (supportId != null)
            {
                _userConnections[supportId] = Context.ConnectionId;
                _availableSupportAgents.Enqueue(supportId);
                await Clients.Caller.SendAsync("RegisterSuccess", "Support registered successfully");
            }
            else
            {
                Console.WriteLine("SupportID not found in token.");
            }
        }

        public async Task SendMessageToSupport(string message)
        {
            var userId = Context.User?.FindFirst("UserID")?.Value;

            if (userId == null)
            {
                Console.WriteLine("UserID not found in token.");
                return;
            }

            if (_availableSupportAgents.TryPeek(out var supportId) &&
                _userConnections.TryGetValue(supportId, out var supportConn))
            {
                await Clients.Client(supportConn).SendAsync("ReceiveMessage", userId, message);
            }
        }

        public async Task SendMessageToUser(string userId, string message)
        {
            if (_userConnections.TryGetValue(userId, out var userConn))
            {
                await Clients.Client(userConn).SendAsync("ReceiveMessage", "support", message);
            }
            else
            {
                Console.WriteLine($"User with ID {userId} not found in connections.");
            }
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var user = _userConnections.FirstOrDefault(kv => kv.Value == Context.ConnectionId);
            if (!string.IsNullOrEmpty(user.Key))
            {
                _userConnections.TryRemove(user.Key, out _);

                // Remove support agent from available queue if they disconnect
                if (_availableSupportAgents.Contains(user.Key))
                {
                    _availableSupportAgents = new ConcurrentQueue<string>(_availableSupportAgents.Where(agent => agent != user.Key));
                }
            }

            return base.OnDisconnectedAsync(exception);
        }
    }


}
