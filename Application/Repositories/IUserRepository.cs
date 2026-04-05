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

    /// <summary>
    ///     Returns whether a user exists or not
    /// </summary>
    /// <param name="id">The user's Guid</param>
    /// <param name="ct">The cancellation token</param>
    /// <returns>True if the user exists, false otherwise</returns>
    Task<bool> ExistsAsync(Guid id, CancellationToken ct);

    /// <summary>
    ///     Creates a user
    /// </summary>
    /// <param name="user">The user to create</param>
    /// <param name="ct">The cancellation token</param>
    Task CreateAsync(User user, CancellationToken ct);
}