using Meetline.Modules.Users.Application.Services;
using Meetline.Modules.Users.Application.Users.Commands.UpsertUser;
using Wolverine;

namespace Meetline.Modules.Users.Application.Users.Commands.SyncUserFromIdentityProvider;

public static class SyncUserFromIdentityProviderCommandHandler
{
    public static async Task<Guid> Handle(
        SyncUserFromIdentityProviderCommand command,
        IIdentityProviderClientService identityProvider,
        IMessageBus bus,
        CancellationToken ct)
    {
        var result = await identityProvider.GetUser(command.ExternalId, ct);

        if (result.IsFailed)
            throw new InvalidOperationException(
                $"Failed to sync user from identity provider: {result.Errors.FirstOrDefault()?.Message}");

        return await bus.InvokeAsync<Guid>(new UpsertUserCommand(result.Value), ct);
    }
}