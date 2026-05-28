using Microsoft.AspNetCore.SignalR;
using Web.Auth;
using Web.Scopes;

namespace Web.Hubs.Filters;

public class HubCallerContextFilter : IHubFilter
{
    public async Task OnConnectedAsync(HubLifetimeContext context, Func<HubLifetimeContext, Task> next)
    {
        PopulateContext(context.Context, context.ServiceProvider);
        await next(context);
    }

    public async Task<object?> InvokeMethodAsync(
        HubInvocationContext invocationContext,
        Func<HubInvocationContext, Task<object?>> next)
    {
        PopulateContext(invocationContext.Context, invocationContext.ServiceProvider);
        return await next(invocationContext);
    }

    private static void PopulateContext(HubCallerContext callerContext, IServiceProvider serviceProvider)
    {
        var user = callerContext.User;

        if (user?.Identity?.IsAuthenticated != true) return;

        var internalIdClaim = user.FindFirst(InternalUserIdClaimsTransformation.ClaimType)?.Value
                              ?? throw new HubException("Authenticated user is missing an internal_id claim");

        if (!Guid.TryParse(internalIdClaim, out var internalId))
            throw new HubException("internal_id claim is not a valid GUID");

        var accessor = serviceProvider.GetRequiredService<ICallerContextAccessor>();
        accessor.CallerContext = new WebProvidedCallerContext(internalId);
    }
}