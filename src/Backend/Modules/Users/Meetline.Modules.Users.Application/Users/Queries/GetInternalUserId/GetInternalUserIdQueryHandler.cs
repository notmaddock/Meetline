using Meetline.Modules.Users.Application.Data;
using Meetline.Modules.Users.Application.Users.Commands.SyncUserFromIdentityProvider;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Wolverine;

namespace Meetline.Modules.Users.Application.Users.Queries.GetInternalUserId;

public static class GetInternalUserIdQueryHandler
{
    public static async Task<Guid> Handle(
        GetInternalUserIdQuery query,
        IUsersDbContext context,
        IMessageBus bus,
        HybridCache cache,
        CancellationToken ct)
    {
        return await cache.GetOrCreateAsync(
            $"user-{query.ExternalId}",
            async cancel =>
            {
                var userId = await context.Users
                    .AsNoTracking()
                    .Where(u => u.ExternalId == query.ExternalId)
                    .Select(u => (Guid?)u.Id)
                    .FirstOrDefaultAsync(cancel);

                if (userId.HasValue) return userId.Value;

                return await bus.InvokeAsync<Guid>(new SyncUserFromIdentityProviderCommand(query.ExternalId), cancel);
            },
            new HybridCacheEntryOptions { Flags = HybridCacheEntryFlags.None },
            tags: [$"user-{query.ExternalId}"],
            cancellationToken: ct);
    }
}