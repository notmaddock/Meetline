using Meetline.Modules.Users.Application.Users.DTOs.UserResponse;
using Meetline.Modules.Users.Application.Users.Queries.GetUserById;
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
    }
}