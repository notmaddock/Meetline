using FluentResults;
using Mediator;
using Meetline.Modules.SharedKernel.Application.Errors.ErrorTypes;
using Meetline.Modules.Users.Application.Users.Commands.SyncUserFromIdentityProvider;
using Meetline.Modules.Users.Application.Users.DTOs.UserGuidResponse;
using Meetline.Modules.Users.Application.Users.Queries.GetUserIdByExternalId;

namespace Meetline.Modules.Users.Application.Users.Queries.ResolveUserIdFromExternalId;

public class ResolveUserIdFromExternalIdCommandHandler(
    IMediator mediator)
    : ICommandHandler<ResolveUserIdFromExternalIdCommand, Result<UserGuidResponse>>
{
    public async ValueTask<Result<UserGuidResponse>> Handle(
        ResolveUserIdFromExternalIdCommand command, CancellationToken cancellationToken)
    {
        // Quick path - user already synced once, bail out early
        var internalIdResult =
            await mediator.Send(new GetUserIdByExternalIdQuery(command.ExternalId), cancellationToken);

        if (internalIdResult.IsSuccess) return Result.Ok(new UserGuidResponse(internalIdResult.Value.Id));

        // Bail out if failed for any reason other than the user not existing
        if (!internalIdResult.HasError<NotFoundError>()) return Result.Fail(internalIdResult.Errors);

        // IdP sync
        var syncResult =
            await mediator.Send(new SyncUserFromIdentityProviderCommand(command.ExternalId), cancellationToken);

        if (syncResult.IsFailed) return Result.Fail(syncResult.Errors);

        return Result.Ok(new UserGuidResponse(syncResult.Value.Id));
    }
}