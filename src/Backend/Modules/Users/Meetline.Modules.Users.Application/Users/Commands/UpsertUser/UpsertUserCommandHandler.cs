using Meetline.Modules.Users.Application.Repositories;
using Meetline.Modules.Users.Application.Users.Mappers;
using Microsoft.Extensions.Caching.Hybrid;

namespace Meetline.Modules.Users.Application.Users.Commands.UpsertUser;

public static class UpsertUserCommandHandler
{
    public static async Task<Guid> Handle(UpsertUserCommand command, IUserRepository userRepository, HybridCache cache, CancellationToken ct)
    {
        var user = UserMapper.ToEntity(command.Data);
        var upsertedUser = await userRepository.UpsertByExternalIdAsync(user, ct);
        await cache.RemoveByTagAsync($"user-{command.Data.ExternalId}", ct);
        return upsertedUser.Id;
    }
}