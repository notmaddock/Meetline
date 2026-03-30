using Application.Repositories;
using FluentResults;
using Mediator;

namespace Application.Features.User.Exists;

public class ExistsHandler(IUserRepository repository) : IQueryHandler<ExistsQuery, Result<ExistsResponse>>
{
    public async ValueTask<Result<ExistsResponse>> Handle(ExistsQuery query, CancellationToken cancellationToken)
    {
        // TODO: Properly cache this when I have the pipeline behavior set up

        var exists = await repository.ExistsAsync(query.Id, cancellationToken);

        return exists ? Result.Ok() : Result.Fail(ExistsErrors.UserNotFoundError(query.Id));
    }
}