using System.Security.Claims;
using Meetline.Modules.Users.Application.Users.Queries.GetInternalUserId;
using Microsoft.AspNetCore.SignalR;
using Web.Scopes;
using Wolverine;

namespace Web.Hubs.Filters;

public class IdentityResolutionFilter : IHubFilter
{
    public async Task OnConnectedAsync(HubLifetimeContext context, Func<HubLifetimeContext, Task> next)
    {
        var user = context.Context.User;

        if (user?.Identity?.IsAuthenticated == true)
        {
            var externalId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrEmpty(externalId))
            {
                var httpContext = context.Context.GetHttpContext();

                if (httpContext != null)
                {
                    var bus = httpContext.RequestServices.GetRequiredService<IMessageBus>();
                    var internalId = await bus.InvokeAsync<Guid>(new GetInternalUserIdQuery(externalId));
                    context.Context.Items["HubUser"] = new WebProvidedCallerContext(internalId, externalId);
                }
            }
        }

        await next(context);
    }
}