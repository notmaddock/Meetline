using Meetline.Modules.Users.Application.Data;
using Meetline.Modules.Users.Application.Repositories;
using Meetline.Modules.Users.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Meetline.Modules.Users.Infrastructure.Repositories;

public class UserRepository(IUsersDbContext ctx) : IUserRepository
{
    public async Task<User> UpsertByExternalIdAsync(User user, CancellationToken ct)
    {
        var now = DateTime.UtcNow;

        user.CreatedAt = now;
        user.UpdatedAt = now;

        var results = await ctx.Users
            .Upsert(user)
            .On(u => u.ExternalId)
            .Exclude(u => new { u.Id, u.CreatedAt }) // don't overwrite the ID or original creation date on update
            .RunAndReturnAsync(ct);

        return results.Single();
    }
}