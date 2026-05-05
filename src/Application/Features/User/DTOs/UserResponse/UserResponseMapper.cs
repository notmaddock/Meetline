using Riok.Mapperly.Abstractions;

namespace Application.Features.User.DTOs.UserResponse;

[Mapper]
public partial class UserResponseMapper
{
    [MapperIgnoreSource(nameof(Domain.Entities.User.ExternalId))]
    public partial UserResponse ToResponse(Domain.Entities.User user);
}