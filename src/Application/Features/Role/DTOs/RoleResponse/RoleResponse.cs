using Domain.Wrappers;

namespace Application.Features.Role.DTOs.RoleResponse;

public record RoleResponse(Guid Id, string Name, bool Hoist, int Position, PermissionSet Permissions);