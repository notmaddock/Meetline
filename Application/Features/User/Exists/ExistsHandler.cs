using Application.Features.User.Errors;
using Application.Repositories;
using FluentResults;
using Mediator;

namespace Application.Features.User.Exists;

public class ExistsHandler(IUserRepository repository) : IQueryHandler<ExistsQuery, Result<ExistsResponse>>
{
    public async ValueTask<Result<ExistsResponse>> Handle(ExistsQuery query, CancellationToken cancellationToken)
    {
        var exists = await repository.ExistsAsync(query.Id, cancellationToken);

        return exists ? Result.Ok() : Result.Fail(GenericUserErrors.UserNotFoundError(query.Id));
    }
}