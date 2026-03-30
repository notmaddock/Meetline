using Application.Errors;

namespace Application.Features.Generic.User;

public class GenericUserErrors
{
    /// <summary>
    ///     A generic not found error for User entities
    /// </summary>
    /// <param name="id">The user's ID</param>
    /// <returns>An ApplicationError explaining the situation</returns>
    public static ApplicationError UserNotFoundError(Guid id)
    {
        return ApplicationError.NotFound("User.NotFound", "User not found", $"User with ID {id} not found");
    }

    /// <summary>
    ///     A not found error for User entities with a hint regarding user onboarding. Useful when there's a high likelihood
    ///     that the reason the user wasn't found is that they are not onboard.
    /// </summary>
    /// <param name="id">The user's ID</param>
    /// <returns>An ApplicationError explaining the situation</returns>
    public static ApplicationError UserNotFoundPerhapsNotOnboardError(Guid id)
    {
        return ApplicationError.Forbidden("User.NotOnboard", "User is not onboard",
            $"User with ID {id} not found, maybe they're not onboard?");
    }
}