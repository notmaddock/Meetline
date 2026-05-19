using Meetline.Modules.Users.Application.Users.DTOs.UserResponse;
using Meetline.Modules.Users.Application.Users.Queries.GetCurrentUser;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace Web.Endpoints.V1;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var users = app.MapGroup("")
            .WithTags("Users");

        users.MapGet("me", GetCurrentUser);
    }

    private static async Task<Results<Ok<UserResponse>, NotFound>> GetCurrentUser(IMessageBus bus)
    {
        var user = await bus.InvokeAsync<UserResponse?>(new GetCurrentUserQuery());

        if (user is null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(user);
    }
}