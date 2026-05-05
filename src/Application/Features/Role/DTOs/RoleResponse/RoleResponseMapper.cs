using Riok.Mapperly.Abstractions;

namespace Application.Features.Role.DTOs.RoleResponse;

[Mapper]
public partial class RoleResponseMapper
{
    [MapperIgnoreSource(nameof(Domain.Entities.Role.CreatedAt))]
    [MapperIgnoreSource(nameof(Domain.Entities.Role.UpdatedAt))]
    public partial RoleResponse ToResponse(Domain.Entities.Role role);

    public partial List<RoleResponse> ToResponse(ICollection<Domain.Entities.Role> roles);
}