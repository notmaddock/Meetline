using Application.Errors;

namespace Application.Features.Role;

public static class RoleErrors
{
    public static ApplicationError RoleNotFound(Guid id)
    {
        return ApplicationError.NotFound("Roles.NotFound", "Role not found", $"The role with ID {id} was not found.");
    }
}