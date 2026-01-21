using Microsoft.AspNetCore.SignalR;

namespace TrackerAPI.Hubs
{

    public class TrackingHub : Hub
    {

        // This class manages the WebSocket connections.
        // We don't need methods here yet, because the *Server* triggers the sending,
        // not the client.
        public override async Task OnConnectedAsync()
        {
            Console.WriteLine($"Client Connected: {Context.ConnectionId}");
            await base.OnConnectedAsync();
        }
    }
}