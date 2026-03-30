using Application.Errors;
using Application.Features.Generic.User;

namespace Application.Features.User.GetUserIdByExternalId;

public static class GetUserIdByExternalIdErrors
{
    public static ApplicationError UserNotFoundError(string id)
    {
        return GenericUserErrors.UserNotFoundError(id);
    }
}