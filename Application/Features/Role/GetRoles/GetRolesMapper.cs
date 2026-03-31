using Riok.Mapperly.Abstractions;

namespace Application.Features.Role.GetRoles;

[Mapper]
public partial class GetRolesMapper
{
    [MapperIgnoreSource(nameof(Domain.Entities.Role.CreatedAt))]
    [MapperIgnoreSource(nameof(Domain.Entities.Role.UpdatedAt))]
    public partial GetRolesResponse ToResponse(Domain.Entities.Role role);

    public partial List<GetRolesResponse> ToResponse(List<Domain.Entities.Role> roles);
}