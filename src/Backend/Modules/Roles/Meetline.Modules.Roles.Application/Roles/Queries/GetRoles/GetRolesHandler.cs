using Application.Features.Role.DTOs.RoleResponse;

namespace Application.Features.Role.GetRoles;

public class GetRolesHandler(IRoleRepository repository)
    : IQueryHandler<GetRolesQuery, Result<ICollection<RoleResponse>>>
{
    private readonly RoleResponseMapper _mapper = new();

    public async ValueTask<Result<ICollection<RoleResponse>>> Handle(GetRolesQuery query,
        CancellationToken cancellationToken)
    {
        var roles = await repository.GetRolesAsync(cancellationToken);

        return _mapper.ToResponse(roles);
    }
}