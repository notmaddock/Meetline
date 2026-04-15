using Application.Errors.ErrorTypes;

namespace Application.Features.User.Errors;

public class UserNotFoundError : NotFoundError
{
    public UserNotFoundError(string externalId) : base("User.NotFound", "User not found",
        $"User with ID {externalId} not found.")
    {
    }

    public UserNotFoundError(Guid id) : this(id.ToString())
    {
    }
}