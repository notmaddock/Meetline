using Meetline.Modules.Users.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Meetline.Modules.Users.Application.Data;

public interface IUsersDbContext
{
    DbSet<User> Users { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}