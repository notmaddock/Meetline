using Application.Repositories;
using FluentResults;
using Mediator;

namespace Application.Features.Role.GetRoleById;

public class GetRoleByIdHandler(IRoleRepository repository)
    : IQueryHandler<GetRoleByIdQuery, Result<GetRoleByIdResponse>>
{
    private static readonly GetRoleByIdMapper Mapper = new();

    public async ValueTask<Result<GetRoleByIdResponse>> Handle(GetRoleByIdQuery query,
        CancellationToken cancellationToken)
    {
        var role = await repository.GetRoleByIdAsync(query.Id, cancellationToken);

        if (role is null) return Result.Fail(RoleErrors.RoleNotFound(query.Id));

        return Mapper.ToResponse(role);
    }
}