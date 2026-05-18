using Meetline.Modules.Users.Application.Data;
using Microsoft.EntityFrameworkCore;

namespace Meetline.Modules.Users.Application.Users.Commands.DeleteUserByExternalId;

public static class DeleteUserByExternalIdCommandHandler
{
    public static Task Handle(DeleteUserByExternalIdCommand command,
        IUsersDbContext context,
        CancellationToken cancellationToken)
    {
        return context.Users
            .Where(u => u.ExternalId == command.ExternalId)
            .ExecuteDeleteAsync(cancellationToken); 
    }
}