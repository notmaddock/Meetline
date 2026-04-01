using System.Security.Claims;
using Application.Features.User.GetUserIdByExternalId;
using Microsoft.AspNetCore.Http.HttpResults;
using Web.Configs;
using Web.Extensions;
using Web.Scopes;

namespace Web.Filters;

public class UserScopeInitializationFilter(Mediator.Mediator sender, CurrentUserScope scope) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        if (context.HttpContext.User.Identity?.IsAuthenticated != true) return await next(context);

        var endpointAllowsNonRegistered =
            context.HttpContext.GetEndpoint()?.Metadata.GetMetadata<AllowNonRegisteredMetadata>() is not null;
        if (endpointAllowsNonRegistered) return await next(context);

        var externalId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                         context.HttpContext.User.FindFirst("sub")?.Value;

        if (externalId is null) return ForbidWithOnboardingHeader(context);

        if (!context.HttpContext.Items.TryGetValue(AuthenticationConfig.TenantIdHttpContextKey, out var rawTenantId) ||
            rawTenantId is not Guid tenantId)
            return TypedResults.Unauthorized();

        var result = await sender.Send(new GetUserIdByExternalIdQuery(externalId));

        if (result.IsSuccess)
            scope.Populate(result.Value.Id, externalId, tenantId);
        else
            return ForbidWithOnboardingHeader(context);

        return await next(context);
    }

    private static ForbidHttpResult ForbidWithOnboardingHeader(EndpointFilterInvocationContext context)
    {
        context.HttpContext.Response.Headers["X-Onboarding-Required"] = "true";
        return TypedResults.Forbid();
    }
}