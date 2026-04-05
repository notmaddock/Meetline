using Application.Errors;

namespace Application.Features.Role.Errors;

public static class GenericRoleErrors
{
    public static ApplicationError RoleNotFound(Guid id)
    {
        return ApplicationError.NotFound("Roles.NotFound", "Role not found", $"The role with ID {id} was not found.");
    }
}