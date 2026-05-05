using Domain.Wrappers;

namespace Application.Features.Role.DTOs.CreateRoleRequest;

public record CreateRoleRequest
{
    public required string Name { get; init; }
    public required bool Hoist { get; init; }
    public required int Position { get; init; }
    public required PermissionSet Permissions { get; init; }
}