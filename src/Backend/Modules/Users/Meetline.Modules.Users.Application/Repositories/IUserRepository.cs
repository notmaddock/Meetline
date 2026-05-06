using Meetline.Modules.Users.Domain.Entities;

namespace Meetline.Modules.Users.Application.Repositories;

public interface IUserRepository
{
    Task<User> UpsertByExternalIdAsync(User user, CancellationToken cancellationToken);
}