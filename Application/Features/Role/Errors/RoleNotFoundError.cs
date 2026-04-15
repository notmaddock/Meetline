using Application.Errors.ErrorTypes;

namespace Application.Features.Role.Errors;

public class RoleNotFoundError(Guid id)
    : NotFoundError("Role.NotFound", "Role not found", "Role with ID {id} not found");