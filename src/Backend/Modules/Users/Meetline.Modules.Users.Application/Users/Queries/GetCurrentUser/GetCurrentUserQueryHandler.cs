using Meetline.Modules.SharedKernel.Application.Context;
using Meetline.Modules.Users.Application.Data;
using Meetline.Modules.Users.Application.Users.DTOs.UserResponse;
using Microsoft.EntityFrameworkCore;

namespace Meetline.Modules.Users.Application.Users.Queries.GetCurrentUser;

public static class GetCurrentUserQueryHandler
{
    public static Task<UserResponse?> Handle(GetCurrentUserQuery _, 
        ICallerContext caller, 
        IUsersDbContext context,
        CancellationToken cancellationToken)
    {
        return context.Users
            .AsNoTracking()
            .Where(u => u.Id == caller.UserId)
            .Select(u => UserResponseMapper.ToResponse(u))
            .FirstOrDefaultAsync(cancellationToken);
    }
}