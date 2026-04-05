using Application.Common.Caching;
using Application.Common.Caching.Keys;
using Application.Features.User.DTOs.CreateUserRequest;
using Application.Features.User.DTOs.UserResponse;
using FluentResults;
using Mediator;

namespace Application.Features.User.OnboardUser;

public record OnboardUserCommand(
    CreateUserRequest Request,
    string ExternalId) : ICommand<Result<UserResponse>>, IInvalidateCacheRequest
{
    public string[] CacheKeysToInvalidate => [UserCacheKeys.ByExternalId(ExternalId)];
}