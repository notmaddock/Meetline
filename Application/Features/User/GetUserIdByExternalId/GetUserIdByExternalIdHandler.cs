using Application.Features.User.DTOs.UserGuidResponse;
using Application.Features.User.Errors;
using Application.Repositories;
using FluentResults;
using Mediator;

namespace Application.Features.User.GetUserIdByExternalId;

public class GetUserIdByExternalIdHandler(IUserRepository repository)
    : IQueryHandler<GetUserIdByExternalIdQuery, Result<UserGuidResponse>>
{
    public async ValueTask<Result<UserGuidResponse>> Handle(GetUserIdByExternalIdQuery query,
        CancellationToken cancellationToken)
    {
        var id = await repository.GetUserIdFromExternalId(query.ExternalId, cancellationToken);

        return id is null
            ? Result.Fail(new UserNotFoundError(query.ExternalId))
            : Result.Ok(new UserGuidResponse(id.Value));
    }
}