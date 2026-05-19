using Meetline.Modules.Users.Application.Data;
using Microsoft.EntityFrameworkCore;

namespace Meetline.Modules.Users.Application.Users.Commands.DeleteUser;

public static class DeleteUserCommandHandler
{
    public static Task Handle(DeleteUserCommand command, IUsersDbContext context, CancellationToken ct)
    {
        return context.Users
            .Where(u => u.ExternalId == command.ExternalId)
            .ExecuteDeleteAsync(ct);
    }
}