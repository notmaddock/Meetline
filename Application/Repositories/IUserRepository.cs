using Domain.Entities;

namespace Application.Repositories;

public interface IUserRepository
{
    /// <summary>
    ///     Gets a User from their ID
    /// </summary>
    /// <param name="id">The user's Guid</param>
    /// <param name="ct">The cancellation token</param>
    /// <returns>The user if found, null otherwise</returns>
    public Task<User?> GetUserById(Guid id, CancellationToken ct);

    /// <summary>
    ///     Gets a User's ID from their external (IdP) ID
    /// </summary>
    /// <param name="externalId">The external ID to query</param>
    /// <param name="ct">The cancellation token</param>
    /// <returns></returns>
    public Task<Guid?> GetUserIdFromExternalId(string externalId, CancellationToken ct);
}