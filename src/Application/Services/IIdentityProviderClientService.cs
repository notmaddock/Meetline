using Application.Features.User.DTOs.UserSyncData;
using FluentResults;

namespace Application.Services;

public interface IIdentityProviderClientService
{
    Task<Result<UserSyncData>> GetUser(string externalId, CancellationToken ct);
}