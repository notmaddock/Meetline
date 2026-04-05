using Application.Errors;
using Application.Features.User.Errors;

namespace Application.Features.User.GetUserIdByExternalId;

public static class GetUserIdByExternalIdErrors
{
    public static ApplicationError UserNotFoundError(string id)
    {
        return GenericUserErrors.UserNotFoundError(id);
    }
}