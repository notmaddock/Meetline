using Meetline.Modules.Users.Application.Users.DTOs.UserResponse;
using Meetline.Modules.Users.Application.Users.Queries.GetCurrentUser;
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

    private static Task<UserResponse> GetCurrentUser(IMessageBus bus)
    {
        return bus.InvokeAsync<UserResponse>(new GetCurrentUserQuery());
    }
}