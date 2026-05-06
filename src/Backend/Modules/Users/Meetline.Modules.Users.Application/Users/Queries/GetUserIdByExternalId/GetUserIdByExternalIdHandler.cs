using FluentResults;
using Mediator;
using Meetline.Modules.Users.Application.Users.DTOs.UserGuidResponse;
using Meetline.Modules.Users.Application.Users.Errors;
using Meetline.Modules.Users.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Meetline.Modules.Users.Application.Users.Queries.GetUserIdByExternalId;

public class GetUserIdByExternalIdHandler(UsersDbContext context)
    : IQueryHandler<GetUserIdByExternalIdQuery, Result<UserGuidResponse>>
{
    public async ValueTask<Result<UserGuidResponse>> Handle(GetUserIdByExternalIdQuery query,
        CancellationToken cancellationToken)
    {
        var id = await context.Users
            .Where(u => u.ExternalId == query.ExternalId)
            .Select(u => (Guid?)u.Id)
            .FirstOrDefaultAsync(cancellationToken);

        return id is null
            ? Result.Fail(new UserNotFoundError(query.ExternalId))
            : Result.Ok(new UserGuidResponse(id.Value));
    }
}