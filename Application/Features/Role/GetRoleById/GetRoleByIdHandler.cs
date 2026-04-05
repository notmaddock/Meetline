using Application.Features.Role.DTOs.RoleResponse;
using Application.Features.Role.Errors;
using Application.Repositories;
using FluentResults;
using Mediator;

namespace Application.Features.Role.GetRoleById;

public class GetRoleByIdHandler(IRoleRepository repository)
    : IQueryHandler<GetRoleByIdQuery, Result<RoleResponse>>
{
    private readonly RoleResponseMapper _mapper = new();

    public async ValueTask<Result<RoleResponse>> Handle(GetRoleByIdQuery query,
        CancellationToken cancellationToken)
    {
        var role = await repository.GetRoleByIdAsync(query.Id, cancellationToken);

        if (role is null) return Result.Fail(GenericRoleErrors.RoleNotFound(query.Id));

        return _mapper.ToResponse(role);
    }
}