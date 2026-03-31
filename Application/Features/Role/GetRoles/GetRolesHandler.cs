using Application.Repositories;
using FluentResults;
using Mediator;

namespace Application.Features.Role.GetRoles;

public class GetRolesHandler(IRoleRepository repository) : IQueryHandler<GetRolesQuery, Result<List<GetRolesResponse>>>
{
    private static readonly GetRolesMapper Mapper = new();

    public async ValueTask<Result<List<GetRolesResponse>>> Handle(GetRolesQuery query,
        CancellationToken cancellationToken)
    {
        var roles = await repository.GetRolesAsync(cancellationToken);

        return Mapper.ToResponse(roles);
    }
}