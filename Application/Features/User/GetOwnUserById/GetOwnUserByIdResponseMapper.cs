using Riok.Mapperly.Abstractions;

namespace Application.Features.User.GetOwnUserById;

[Mapper]
public partial class GetOwnUserByIdResponseMapper
{
    [MapperIgnoreSource(nameof(Domain.Entities.User.ExternalId))]
    [MapperIgnoreSource(nameof(Domain.Entities.User.TenantId))]
    public partial GetOwnUserByIdResponse ToResponse(Domain.Entities.User user);
}