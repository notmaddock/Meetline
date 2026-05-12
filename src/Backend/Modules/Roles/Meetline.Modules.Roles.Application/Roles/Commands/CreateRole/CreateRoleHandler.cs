using FluentResults;
using Mediator;
using Meetline.Modules.Roles.Application.Data;
using Meetline.Modules.Roles.Application.Roles.DTOs.CreateRoleRequest;
using Meetline.Modules.Roles.Application.Roles.DTOs.RoleResponse;

namespace Meetline.Modules.Roles.Application.Roles.Commands.CreateRole;

public class CreateRoleHandler(IRolesDbContext context) : ICommandHandler<CreateRoleCommand, Result<RoleResponse>>
{
    private readonly CreateRoleRequestMapper _requestMapper = new();
    private readonly RoleResponseMapper _responseMapper = new();

    public async ValueTask<Result<RoleResponse>> Handle(CreateRoleCommand command, CancellationToken cancellationToken)
    {
        // TODO permission and position checks
        // TODO graceful failure handling
        // TODO ManageRoles check
        var role = _requestMapper.ToRole(command.Request);

        context.Roles.Add(role);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Ok(_responseMapper.ToResponse(role));
    }
}