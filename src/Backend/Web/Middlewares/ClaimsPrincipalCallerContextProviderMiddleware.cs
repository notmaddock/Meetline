using System.Security.Claims;
using Meetline.Modules.SharedKernel.Application.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Web.Scopes;

namespace Web.Middlewares;

public class ClaimsPrincipalCallerContextProviderMiddleware
{
    public static ICallerContext Before(IHttpContextAccessor accessor)
    {
        var context = accessor.HttpContext;

        if (context is null) throw new UnauthorizedAccessException();

        var externalId = context.User.FindFirst(ClaimTypes.NameIdentifier);
        
        if (externalId is null) throw new UnauthorizedAccessException();
        
        // TODO synchronous sync with IdP
        
        return new WebProvidedCallerContext(Guid.NewGuid(), externalId.Value);
    }
}