using Application.Repositories;
using FluentResults;
using Mediator;

namespace Application.Features.User.GetUserIdByExternalId;

public class GetUserIdByExternalIdHandler(IUserRepository repository)
    : IQueryHandler<GetUserIdByExternalIdQuery, Result<GetUserIdByExternalIdResponse>>
{
    public async ValueTask<Result<GetUserIdByExternalIdResponse>> Handle(GetUserIdByExternalIdQuery query,
        CancellationToken cancellationToken)
    {
        var id = await repository.GetUserIdFromExternalId(query.ExternalId, cancellationToken);

        return id is null
            ? Result.Fail(GetUserIdByExternalIdErrors.UserNotFoundError(query.ExternalId))
            : Result.Ok(new GetUserIdByExternalIdResponse(id.Value));
    }
}