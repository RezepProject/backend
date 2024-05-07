using Microsoft.AspNetCore.SignalR;

namespace backend.Hubs;

public class ConnectionStateHub : Hub
{
    public override Task OnConnectedAsync()
    {
        // get a unique id, which does not change
        var connectionId = Context.ConnectionId;
        Console.WriteLine($"Connection {connectionId} connected");
        return base.OnConnectedAsync();
    }
}