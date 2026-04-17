using Application.Features.User.Errors;
using Application.Repositories;
using FluentResults;
using Mediator;

namespace Application.Features.User.DeleteUserByExternalId;

public class DeleteUserByExternalIdCommandHandler(IUserRepository repository)
    : ICommandHandler<DeleteUserByExternalIdCommand, Result>
{
    public async ValueTask<Result> Handle(DeleteUserByExternalIdCommand byExternalIdCommand,
        CancellationToken cancellationToken)
    {
        var user = await repository.GetUserByExternalId(byExternalIdCommand.ExternalId, cancellationToken);

        if (user is null) return Result.Fail(new UserNotFoundError(byExternalIdCommand.ExternalId));

        await repository.DeleteUser(user, cancellationToken);

        return Result.Ok();
    }
}