namespace Application.Features.Role.GetRoles;

public record GetRolesResponse(Guid Id, string Name, bool Hoist, int Position, PermissionSet Permissions);