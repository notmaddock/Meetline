using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Web.Hubs;

[Authorize]
public class GatewayHub : Hub
{
    public Task<string> Ping()
    {
        return Task.FromResult($"pong {Context.UserIdentifier}");
    }
}