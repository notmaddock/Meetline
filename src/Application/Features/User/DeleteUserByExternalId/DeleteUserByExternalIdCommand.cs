using FluentResults;
using Mediator;

namespace Application.Features.User.DeleteUserByExternalId;

public record DeleteUserByExternalIdCommand(string ExternalId) : ICommand<Result>;