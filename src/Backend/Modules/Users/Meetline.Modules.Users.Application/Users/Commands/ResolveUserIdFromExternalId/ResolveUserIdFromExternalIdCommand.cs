using FluentResults;
using Mediator;
using Meetline.Modules.Users.Application.Users.DTOs.UserGuidResponse;

namespace Meetline.Modules.Users.Application.Users.Queries.ResolveUserIdFromExternalId;

/// <summary>
///     Resolves a user's ID from their external ID. That is, if the user is present in the database it will just return
///     their ID, and otherwise it will try to fetch from the identity provider using
///     <see cref="IIdentityProviderClientService" />.
/// </summary>
public record ResolveUserIdFromExternalIdCommand(string ExternalId)
    : ICommand<Result<UserGuidResponse>>;