using System.Security.Claims;
using Meetline.Modules.Users.Application.Users.Queries.GetInternalUserId;
using Microsoft.AspNetCore.Authentication;
using Wolverine;

namespace Web.Auth;

public class InternalUserIdClaimsTransformation(IMessageBus bus) : IClaimsTransformation
{
    public const string ClaimType = "internal_id";

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        if (principal.HasClaim(m => m.Type == ClaimType) ||
            principal.Identity is not ClaimsIdentity { IsAuthenticated: true })
            return principal;

        var externalId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(externalId)) return principal;

        var response = await bus.InvokeAsync<Guid>(new GetInternalUserIdQuery(externalId));

        var clone = principal.Clone();
        if (clone.Identity is ClaimsIdentity cloneIdentity)
            cloneIdentity.AddClaim(new Claim(ClaimType, response.ToString()));

        return clone;
    }
}