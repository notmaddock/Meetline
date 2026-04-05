using Riok.Mapperly.Abstractions;

namespace Application.Features.User.DTOs.UserPublicResponse;

[Mapper]
public partial class UserPublicResponseMapper
{
    [MapperIgnoreSource(nameof(Domain.Entities.User.ExternalId))]
    [MapperIgnoreSource(nameof(Domain.Entities.User.CreatedAt))]
    [MapperIgnoreSource(nameof(Domain.Entities.User.UpdatedAt))]
    public partial UserPublicResponse ToResponse(Domain.Entities.User user);
}