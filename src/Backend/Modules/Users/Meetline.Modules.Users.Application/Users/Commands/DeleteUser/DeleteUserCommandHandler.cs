using Meetline.Modules.Users.Application.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;

namespace Meetline.Modules.Users.Application.Users.Commands.DeleteUser;

public static class DeleteUserCommandHandler
{
    public static async Task Handle(DeleteUserCommand command, IUsersDbContext context, HybridCache cache, CancellationToken ct)
    {
        await context.Users
            .Where(u => u.ExternalId == command.ExternalId)
            .ExecuteDeleteAsync(ct);

        await cache.RemoveByTagAsync($"user-{command.ExternalId}", ct);
    }
}