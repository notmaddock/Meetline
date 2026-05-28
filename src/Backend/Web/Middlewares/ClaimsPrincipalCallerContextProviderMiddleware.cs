using System.Security.Claims;
using Meetline.Modules.SharedKernel.Application.Context;
using Meetline.Modules.Users.Application.Users.Queries.GetInternalUserId;
using Web.Auth;
using Web.Scopes;
using Wolverine;

namespace Web.Middlewares;

public class ClaimsPrincipalCallerContextProviderMiddleware
{
    public static async Task<ICallerContext> Before(
        ICallerContextAccessor accessor,
        IHttpContextAccessor httpContextAccessor,
        IMessageBus bus)
    {
        if (accessor.CallerContext is not null) return accessor.CallerContext;

        var context = httpContextAccessor.HttpContext;
        if (context is null) throw new UnauthorizedAccessException();

        // Check if internal_id claim exists first
        var internalIdClaim = context.User.FindFirst(InternalUserIdClaimsTransformation.ClaimType);
        if (internalIdClaim is not null && Guid.TryParse(internalIdClaim.Value, out var internalId))
        {
            var callerContext = new WebProvidedCallerContext(internalId);
            accessor.CallerContext = callerContext;
            return callerContext;
        }

        // Fallback DB lookup if the claim is missing (e.g. in tests)
        var externalId = context.User.FindFirst(ClaimTypes.NameIdentifier);
        if (externalId is null) throw new UnauthorizedAccessException();

        var dbInternalId = await bus.InvokeAsync<Guid>(new GetInternalUserIdQuery(externalId.Value));
        var dbCallerContext = new WebProvidedCallerContext(dbInternalId);
        accessor.CallerContext = dbCallerContext;
        return dbCallerContext;
    }
}