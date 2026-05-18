namespace Meetline.Modules.Users.Application.Users.Queries.GetUserIdByExternalId;

/// <summary>
///     A query for getting a user's internal ID from their external ID
/// </summary>
/// <param name="ExternalId">The user's external ID</param>
public record GetUserIdByExternalIdQuery(string ExternalId);