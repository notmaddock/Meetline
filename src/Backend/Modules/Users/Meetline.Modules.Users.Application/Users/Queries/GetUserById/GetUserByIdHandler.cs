using FluentResults;
using Meetline.Modules.Users.Application.Data;
using Meetline.Modules.Users.Application.Users.DTOs.UserResponse;
using Meetline.Modules.Users.Application.Users.Errors;

namespace Meetline.Modules.Users.Application.Users.Queries.GetUserById;

public static class GetUserByIdHandler
{ 
    public static async ValueTask<Result<UserResponse>> Handle(GetUserByIdQuery query,
        IUsersDbContext context,
        CancellationToken cancellationToken)
    {
        var user = await context.Users.FindAsync([query.Id], cancellationToken);

        return user is null
            ? Result.Fail(new UserNotFoundError(query.Id))
            : Result.Ok(UserResponseMapper.ToResponse(user));
    }
}