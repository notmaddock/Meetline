using Application.Errors.ErrorTypes;

namespace Application.Features.User.Errors;

public class UserNotFoundError(string externalId) : NotFoundError("User.NotFound", "User not found",
    $"User with ID {externalId} not found.")
{
    public UserNotFoundError(Guid id) : this(id.ToString())
    {
    }
}