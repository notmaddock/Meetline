using Riok.Mapperly.Abstractions;

namespace Application.Features.User.DTOs.UserSyncData;

[Mapper]
public partial class UserSyncDataMapper
{
    [MapperIgnoreTarget(nameof(Domain.Entities.User.Id))]
    [MapperIgnoreTarget(nameof(Domain.Entities.User.CreatedAt))]
    [MapperIgnoreTarget(nameof(Domain.Entities.User.UpdatedAt))]
    public partial Domain.Entities.User ToUser(UserSyncData idpUserSyncData);
}