using Meetline.Modules.Users.Application.Repositories;
using Meetline.Modules.Users.Application.Users.Events;
using Meetline.Modules.Users.Application.Users.Mappers;

namespace Meetline.Modules.Users.Application.Users.Commands.UpsertUser;

public static class UpsertUserCommandHandler
{
    public static async Task<UserProfileSyncedEvent> Handle(
        UpsertUserCommand command,
        IUserRepository userRepository,
        CancellationToken ct)
    {
        var user = UserMapper.ToEntity(command.Data);
        var upsertedUser = await userRepository.UpsertByExternalIdAsync(user, ct);
        return new UserProfileSyncedEvent(upsertedUser.Id, upsertedUser.ExternalId);
    }
}