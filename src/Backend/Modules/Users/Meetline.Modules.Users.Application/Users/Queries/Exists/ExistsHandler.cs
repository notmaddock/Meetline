using FluentResults;
using Mediator;
using Meetline.Modules.Users.Application.Users.Errors;
using Meetline.Modules.Users.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Meetline.Modules.Users.Application.Users.Queries.Exists;

public class ExistsHandler(UsersDbContext context) : IQueryHandler<ExistsQuery, Result<ExistsResponse>>
{
    public async ValueTask<Result<ExistsResponse>> Handle(ExistsQuery query, CancellationToken cancellationToken)
    {
        var exists = await context.Users
            .AnyAsync(u => u.Id == query.Id, cancellationToken);

        return exists ? Result.Ok() : Result.Fail(new UserNotFoundError(query.Id));
    }
}