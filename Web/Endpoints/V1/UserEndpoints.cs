using Application.Features.User.DTOs.UserPublicResponse;
using Application.Features.User.DTOs.UserResponse;
using Application.Features.User.GetOwnUserById;
using Application.Features.User.GetUserById;
using Microsoft.AspNetCore.Http.HttpResults;
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
}