using FluentResults;
using Mediator;
using Meetline.Modules.Roles.Application.Data;
using Meetline.Modules.Roles.Application.Roles.DTOs.RoleResponse;
using Meetline.Modules.Roles.Application.Roles.Errors;

namespace Meetline.Modules.Roles.Application.Roles.Queries.GetRoleById;

public class GetRoleByIdHandler(IRolesDbContext context)
    : IQueryHandler<GetRoleByIdQuery, Result<RoleResponse>>
{
    private readonly RoleResponseMapper _mapper = new();

    public async ValueTask<Result<RoleResponse>> Handle(GetRoleByIdQuery query,
        CancellationToken cancellationToken)
    {
        var role = await context.Roles.FindAsync([query.Id], cancellationToken);

        if (role is null) return Result.Fail(new RoleNotFoundError(query.Id));

        return _mapper.ToResponse(role);
    }
}