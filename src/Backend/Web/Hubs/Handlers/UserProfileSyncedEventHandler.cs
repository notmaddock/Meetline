using Meetline.Modules.Users.Application.Users.Events;
using Microsoft.AspNetCore.SignalR;

namespace Web.Hubs.Handlers;

public static class UserProfileSyncedEventHandler
{
    public static async Task Handle(
        UserProfileSyncedEvent @event,
        IHubContext<GatewayHub> hubContext,
        ILogger<GatewayHub> logger,
        CancellationToken ct)
    {
        await hubContext.Clients.User(@event.UserId.ToString())
            .SendAsync("UserProfileSynced", ct);
    }
}