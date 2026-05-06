using Meetline.Modules.Users.Application.Repositories;
using Meetline.Modules.Users.Domain.Entities;
using Meetline.Modules.Users.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Meetline.Modules.Users.Infrastructure.Repositories;

internal class UserRepository(UsersDbContext ctx) : IUserRepository
{
    public async Task<User> UpsertByExternalIdAsync(User user, CancellationToken ct)
    {
        var results = await ctx.Users
            .Upsert(user)
            .On(u => u.ExternalId)
            .RunAndReturnAsync(ct);

        return results.Single();
    }
}