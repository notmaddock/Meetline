using Riok.Mapperly.Abstractions;

namespace Application.Features.User.GetUserById;

[Mapper]
public partial class GetUserByIdResponseMapper
{
    [MapperIgnoreSource(nameof(Domain.Entities.User.ExternalId))]
    [MapperIgnoreSource(nameof(Domain.Entities.User.UpdatedAt))]
    [MapperIgnoreSource(nameof(Domain.Entities.User.TenantId))]
    public partial GetUserByIdResponse ToResponse(Domain.Entities.User user);
}