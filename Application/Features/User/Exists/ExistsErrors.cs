using Application.Errors;
using Application.Features.Generic.User;

namespace Application.Features.User.Exists;

public static class ExistsErrors
{
    public static ApplicationError UserNotFoundError(Guid id)
    {
        return GenericUserErrors.UserNotFoundError(id);
    }
}