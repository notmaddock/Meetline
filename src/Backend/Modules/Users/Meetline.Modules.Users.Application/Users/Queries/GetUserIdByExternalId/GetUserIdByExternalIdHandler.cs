using FluentResults;
using Meetline.Modules.Users.Application.Data;
using Meetline.Modules.Users.Application.Users.DTOs.UserGuidResponse;
using Meetline.Modules.Users.Application.Users.Errors;
using Microsoft.EntityFrameworkCore;

namespace Meetline.Modules.Users.Application.Users.Queries.GetUserIdByExternalId;

public static class GetUserIdByExternalIdQueryHandler
{
    public static async ValueTask<Result<UserGuidResponse>> Handle(GetUserIdByExternalIdQuery query,
        IUsersDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var id = await dbContext.Users
            .Where(u => u.ExternalId == query.ExternalId)
            .Select(u => (Guid?)u.Id)
            .FirstOrDefaultAsync(cancellationToken);

        return id is null
            ? Result.Fail(new UserNotFoundError(query.ExternalId))
            : Result.Ok(new UserGuidResponse(id.Value));
    }
}