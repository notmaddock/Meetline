using Riok.Mapperly.Abstractions;

namespace Application.Features.Role.DTOs.CreateRoleRequest;

[Mapper]
public partial class CreateRoleRequestMapper
{
    [MapperIgnoreTarget(nameof(Domain.Entities.Role.Id))]
    [MapperIgnoreTarget(nameof(Domain.Entities.Role.CreatedAt))]
    [MapperIgnoreTarget(nameof(Domain.Entities.Role.UpdatedAt))]
    public partial Domain.Entities.Role ToRole(CreateRoleRequest request);
}