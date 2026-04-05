using System.Security.Claims;
using Application.Features.User.DTOs.CreateUserRequest;
using Application.Features.User.DTOs.UserPublicResponse;
using Application.Features.User.DTOs.UserResponse;
using Application.Features.User.GetOwnUserById;
using Application.Features.User.GetUserById;
using Application.Features.User.OnboardUser;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Web.Extensions;
using Web.Scopes;

namespace Web.Endpoints.V1;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var users = app.MapGroup("")
            .WithTags("Users");

        users.MapGet("/me", GetCurrentUser)
            .WithName("GetCurrentUser")
            .WithSummary("Get the currently authenticated user")
            .WithDescription("Returns the full profile of the user making the request. Requires a valid JWT token.");

        users.MapGet("/{id:guid}", GetUserById)
            .WithName("GetUserById")
            .WithSummary("Get user by ID")
            .WithDescription("Retrieves a user profile by their unique identifier.");

        users.MapPost("/onboard", Onboard)
            .AllowNonRegistered()
            .WithName("OnboardUser")
            .WithSummary("Onboard a new user")
            .WithDescription("""
                             Creates a new user account linked to an external identity (e.g. from Auth0, Firebase, etc.).
                             This endpoint is public and should be called after successful external authentication.
                             """);
    }

    private static async Task<Results<Ok<UserResponse>, ForbidHttpResult, ProblemHttpResult>>
        GetCurrentUser(Mediator.Mediator mediator, CurrentUserScope scope)
    {
        var result = await mediator.Send(new GetOwnUserByIdQuery(scope.Id));

        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : result.ToProblemHttpResult();
    }

    private static async Task<Results<Ok<UserPublicResponse>, NotFound, ProblemHttpResult>>
        GetUserById(Mediator.Mediator mediator, Guid id)
    {
        var result = await mediator.Send(new GetUserByIdQuery(id));

        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : result.ToProblemHttpResult();
    }

    private static async
        Task<Results<Created<UserResponse>, BadRequest, UnauthorizedHttpResult, Conflict, ProblemHttpResult>>
        Onboard(
            Mediator.Mediator mediator,
            HttpContext context,
            [FromBody] CreateUserRequest request)
    {
        var externalId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                         ?? context.User.FindFirst("sub")?.Value;

        if (externalId is null)
            return TypedResults.Unauthorized();

        var command = new OnboardUserCommand(request, externalId);

        var result = await mediator.Send(command);

        return result.IsSuccess
            ? TypedResults.Created($"/api/users/{result.Value.Id}", result.Value)
            : result.ToProblemHttpResult();
    }
}