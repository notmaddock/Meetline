using Application.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository(ApplicationDbContext ctx) : IUserRepository
{
    public Task<User?> GetUserById(Guid id, CancellationToken ct)
    {
        return ctx.Users.FindAsync([id], ct).AsTask();
    }

    public Task<Guid?> GetUserIdFromExternalId(string externalId, CancellationToken ct)
    {
        return ctx.Users
            .Where(u => u.ExternalId == externalId)
            .Select(u => (Guid?)u.Id)
            .FirstOrDefaultAsync(ct);
    }
}