using Riok.Mapperly.Abstractions;

namespace Application.Features.User.DTOs.CreateUserRequest;

[Mapper]
public partial class CreateUserRequestMapper
{
    [MapperIgnoreTarget(nameof(Domain.Entities.User.Id))]
    [MapperIgnoreTarget(nameof(Domain.Entities.User.CreatedAt))]
    [MapperIgnoreTarget(nameof(Domain.Entities.User.UpdatedAt))]
    public partial Domain.Entities.User ToUser(CreateUserRequest request, string externalId);
}