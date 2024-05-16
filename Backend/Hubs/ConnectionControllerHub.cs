using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace backend.Hubs;

[Authorize]
public class ConnectionControllerHub : Hub
{
    public override Task OnConnectedAsync()
    {
        // get a unique id, which does not change
        var connectionId = Context.ConnectionId;
        Console.WriteLine($"Connection {connectionId} connected");
        return base.OnConnectedAsync();
    }
}