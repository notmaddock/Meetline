using System.Security.Claims;
using Application.Features.User.ResolveUserIdFromExternalId;
using Web.Extensions;
using Web.Scopes;

namespace Web.Filters;

public class UserScopeInitializationFilter(Mediator.Mediator sender, CurrentUserScope scope) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var httpContext = context.HttpContext;

        if (httpContext.User.Identity?.IsAuthenticated != true) return await next(context);

        var externalId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                         httpContext.User.FindFirst("sub")?.Value;

        if (externalId is null) return TypedResults.Unauthorized();

        var result = await sender.Send(new ResolveUserIdFromExternalIdCommand(externalId),
            httpContext.RequestAborted);

        if (result.IsFailed) return result.ToProblemHttpResult();

        scope.Populate(result.Value.Id, externalId);
        return await next(context);
    }
}