using System.Security.Claims;
using Application.Features.User.GetOwnUserById;
using Application.Features.User.GetUserById;
using Application.Features.User.OnboardUser;
using Microsoft.AspNetCore.Mvc;
using Web.Extensions;
using Web.Scopes;

namespace Web.Endpoints.V1;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/me", GetCurrentUser);
        app.MapGet("/{id:guid}", GetUserById);
        app.MapPost("/onboard", Onboard).AllowNonRegistered();
    }

    private static async Task<IResult> GetCurrentUser(Mediator.Mediator mediator, CurrentUserScope scope)
    {
        var result = await mediator.Send(new GetOwnUserByIdQuery(scope.UserId));

        return result.IsSuccess ? TypedResults.Ok(result.Value) : result.ToProblemDetails();
    }

    private static async Task<IResult> GetUserById(Mediator.Mediator mediator, Guid id)
    {
        var result = await mediator.Send(new GetUserByIdQuery(id));

        return result.IsSuccess ? TypedResults.Ok(result.Value) : result.ToProblemDetails();
    }

    private static async Task<IResult> Onboard(
        Mediator.Mediator mediator,
        HttpContext context,
        [FromBody] OnboardUserRequest request)
    {
        var externalId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                         context.User.FindFirst("sub")?.Value;

        if (externalId is null) return TypedResults.Unauthorized();

        var result = await mediator.Send(new OnboardUserCommand(
            externalId,
            request.Username,
            request.Email,
            request.FirstName,
            request.LastName));

        return result.IsSuccess
            ? TypedResults.Created($"/api/users/{result.Value}", result.Value)
            : result.ToProblemDetails();
    }

    private record OnboardUserRequest(string Username, string Email, string? FirstName, string? LastName);
}