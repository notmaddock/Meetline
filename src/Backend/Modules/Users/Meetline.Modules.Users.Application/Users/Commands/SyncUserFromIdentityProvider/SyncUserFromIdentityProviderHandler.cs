using FluentResults;
using Mediator;
using Meetline.Modules.Users.Application.Repositories;
using Meetline.Modules.Users.Application.Services;
using Meetline.Modules.Users.Application.Users.DTOs.UserResponse;
using Meetline.Modules.Users.Application.Users.Errors;
using Microsoft.Extensions.Logging;

namespace Meetline.Modules.Users.Application.Users.Commands.SyncUserFromIdentityProvider;

public class SyncUserFromIdentityProviderHandler(
    ILogger<SyncUserFromIdentityProviderHandler> logger,
    IUserRepository repository,
    IIdentityProviderClientService idpClientService)
    : ICommandHandler<SyncUserFromIdentityProviderCommand, Result<UserResponse>>
{
    private readonly UserResponseMapper _responseMapper = new();
    private readonly UserSyncDataMapper _syncDataMapper = new();

    public async ValueTask<Result<UserResponse>> Handle(SyncUserFromIdentityProviderCommand command,
        CancellationToken cancellationToken)
    {
        var idpUserResult = await idpClientService.GetUser(command.ExternalId, cancellationToken);

        if (idpUserResult.IsFailed) return Result.Fail(idpUserResult.Errors);

        var user = _syncDataMapper.ToUser(idpUserResult.Value);

        try
        {
            var upsertedUser = await repository.UpsertByExternalIdAsync(user, cancellationToken);
            return Result.Ok(_responseMapper.ToResponse(upsertedUser));
        }
        catch (Exception e)
        {
            logger.LogError(e, "Couldn't upsert user sync data");
            return Result.Fail(new IdentityProviderSyncError());
        }
    }
}