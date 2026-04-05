using Domain.Wrappers;

namespace Application.Features.Role.DTOs.CreateRoleRequest;

public record CreateRoleRequest(string Name, bool Hoist, int Position, PermissionSet Permissions);