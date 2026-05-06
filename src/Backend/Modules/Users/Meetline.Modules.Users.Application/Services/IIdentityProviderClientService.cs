using FluentResults;

namespace Meetline.Modules.Users.Application.Services;

public interface IIdentityProviderClientService
{
    Task<Result<UserSyncData>> GetUser(string externalId, CancellationToken ct);
}