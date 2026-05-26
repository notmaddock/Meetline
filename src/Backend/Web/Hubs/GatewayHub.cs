using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Web.Scopes;

namespace Web.Hubs;

[Authorize]
public class GatewayHub : Hub
{
    private WebProvidedCallerContext HubUser => Context.Items["HubUser"] as WebProvidedCallerContext
                                                ?? throw new HubException("User caller context missing");

    public Task<string> Ping()
    {
        return Task.FromResult($"pong {HubUser.UserId}/{HubUser.UserExternalId} with new logic!");
    }
}