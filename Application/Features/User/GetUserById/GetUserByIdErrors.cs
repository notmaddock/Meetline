using Application.Errors;
using Application.Features.User.Errors;

namespace Application.Features.User.GetUserById;

public static class GetUserByIdErrors
{
    public static ApplicationError UserNotFoundError(Guid id)
    {
        return GenericUserErrors.UserNotFoundError(id);
    }
}