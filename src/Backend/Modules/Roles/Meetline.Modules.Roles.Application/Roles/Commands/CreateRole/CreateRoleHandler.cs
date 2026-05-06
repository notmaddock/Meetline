using Application.Features.Role.DTOs.CreateRoleRequest;
using Application.Features.Role.DTOs.RoleResponse;

namespace Application.Features.Role.CreateRole;

public class CreateRoleHandler(IRoleRepository repository) : ICommandHandler<CreateRoleCommand, Result<RoleResponse>>
{
    private readonly CreateRoleRequestMapper _requestMapper = new();
    private readonly RoleResponseMapper _responseMapper = new();

    public async ValueTask<Result<RoleResponse>> Handle(CreateRoleCommand command, CancellationToken cancellationToken)
    {
        // TODO permission and position checks
        // TODO graceful failure handling
        // TODO ManageRoles check
        var role = _requestMapper.ToRole(command.Request);

        await repository.CreateRoleAsync(role, cancellationToken);

        return Result.Ok(_responseMapper.ToResponse(role));
    }
}