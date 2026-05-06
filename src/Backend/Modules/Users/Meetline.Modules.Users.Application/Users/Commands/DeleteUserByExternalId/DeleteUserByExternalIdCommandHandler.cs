using FluentResults;
using Mediator;
using Meetline.Modules.Users.Application.Data;
using Meetline.Modules.Users.Application.Users.Errors;
using Microsoft.EntityFrameworkCore;

namespace Meetline.Modules.Users.Application.Users.Commands.DeleteUserByExternalId;

public class DeleteUserByExternalIdCommandHandler(UsersDbContext context)
    : ICommandHandler<DeleteUserByExternalIdCommand, Result>
{
    public async ValueTask<Result> Handle(DeleteUserByExternalIdCommand byExternalIdCommand,
        CancellationToken cancellationToken)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.ExternalId == byExternalIdCommand.ExternalId, cancellationToken);

        if (user is null) return Result.Fail(new UserNotFoundError(byExternalIdCommand.ExternalId));

        context.Users.Remove(user);

        return Result.Ok();
    }
}