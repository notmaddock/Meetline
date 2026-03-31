using Application.Repositories;
using FluentResults;
using Mediator;

namespace Application.Features.Role.CreateRole;

public class CreateRoleHandler(IRoleRepository repository) : ICommandHandler<CreateRoleCommand, Result<Guid>>
{
    public async ValueTask<Result<Guid>> Handle(CreateRoleCommand command, CancellationToken cancellationToken)
    {
        // TODO permission and position checks
        // TODO graceful failure handling
        // TODO ManageRoles check
        var role = new Domain.Entities.Role
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Hoist = command.Hoist,
            Position = command.Position,
            Permissions = command.Permissions
        };

        await repository.AddRoleAsync(role, cancellationToken);

        return Result.Ok(role.Id);
    }
}