using Domain.Wrappers;

namespace Application.Features.Role.GetRoleById;

public record GetRoleByIdResponse(Guid Id, string Name, bool Hoist, int Position, PermissionSet Permissions);