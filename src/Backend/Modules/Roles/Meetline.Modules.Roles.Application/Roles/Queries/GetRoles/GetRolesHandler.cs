using FluentResults;
using Mediator;
using Meetline.Modules.Roles.Application.Data;
using Meetline.Modules.Roles.Application.Roles.DTOs.RoleResponse;
using Microsoft.EntityFrameworkCore;

namespace Meetline.Modules.Roles.Application.Roles.Queries.GetRoles;

public class GetRolesHandler(IRolesDbContext context)
    : IQueryHandler<GetRolesQuery, Result<ICollection<RoleResponse>>>
{
    private readonly RoleResponseMapper _mapper = new();

    public async ValueTask<Result<ICollection<RoleResponse>>> Handle(GetRolesQuery query,
        CancellationToken cancellationToken)
    {
        // TODO: Map directly with a Select to make queries smaller
        var roles = await context.Roles.ToListAsync(cancellationToken);

        return _mapper.ToResponse(roles);
    }
}