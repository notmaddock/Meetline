using Riok.Mapperly.Abstractions;

namespace Application.Features.Role.GetRoleById;

[Mapper]
public partial class GetRoleByIdMapper
{
    [MapperIgnoreSource(nameof(Domain.Entities.Role.CreatedAt))]
    [MapperIgnoreSource(nameof(Domain.Entities.Role.UpdatedAt))]
    [MapperIgnoreSource(nameof(Domain.Entities.Role.TenantId))]
    public partial GetRoleByIdResponse ToResponse(Domain.Entities.Role role);
}