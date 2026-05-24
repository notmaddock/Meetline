using System.Security.Claims;
using Meetline.Modules.Users.Application.Users.Queries.GetInternalUserId;
using Microsoft.AspNetCore.SignalR;
using Web.Scopes;
using Wolverine;

namespace Web.Hubs.Filters;

public class IdentityResolutionFilter(IMessageBus bus) : IHubFilter
{
    public async ValueTask<object?> InvokeMethodAsync(
        HubInvocationContext context,
        Func<HubInvocationContext, ValueTask<object?>> next)
    {
        var user = context.Context.User;

        if (user?.Identity?.IsAuthenticated != true) return await next(context);

        var externalId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(externalId)) return await next(context);

        var internalId = await bus.InvokeAsync<Guid>(new GetInternalUserIdQuery(externalId));

        context.Context.Items["HubUser"] = new WebProvidedCallerContext(internalId, externalId);

        return await next(context);
    }
}