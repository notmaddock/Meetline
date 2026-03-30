using Application.Errors;
using Application.Features.Generic.User;

namespace Application.Features.User.GetOwnUserById;

public static class GetOwnUserByIdErrors
{
    public static ApplicationError UserNotFoundError(Guid id)
    {
        return GenericUserErrors.UserNotFoundPerhapsNotOnboardError(id);
    }
}