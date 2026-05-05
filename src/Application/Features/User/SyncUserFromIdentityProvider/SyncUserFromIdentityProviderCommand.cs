using Application.Features.User.DTOs.UserResponse;
using FluentResults;
using Mediator;

namespace Application.Features.User.SyncUserFromIdentityProvider;

public record SyncUserFromIdentityProviderCommand(string ExternalId) : ICommand<Result<UserResponse>>;