using Meetline.Modules.Users.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Meetline.Modules.Users.Application.Users.DTOs.UserResponse;

[Mapper]
public static partial class UserResponseMapper
{
    [MapperIgnoreSource(nameof(User.ExternalId))]
    public static partial UserResponse ToResponse(User user);
}