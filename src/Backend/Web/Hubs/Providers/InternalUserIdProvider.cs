using Microsoft.AspNetCore.SignalR;
using Web.Auth;

namespace Web.Hubs.Providers;

public class InternalUserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        return connection.User.FindFirst(InternalUserIdClaimsTransformation.ClaimType)?.Value;
    }
}