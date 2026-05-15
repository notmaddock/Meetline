using FluentResults;
using Meetline.Modules.SharedKernel.Application.Errors;
using Meetline.Modules.SharedKernel.Application.Errors.ErrorTypes;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Web.Extensions;

public static class ResultExtensions
{
    /// <summary>
    ///     Converts a failed <see cref="Result{T}" /> into a proper Problem Details response.
    ///     Throws if called on a successful result.
    /// </summary>
    public static ProblemHttpResult ToProblemHttpResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
            throw new InvalidOperationException("ToProblemHttpResult should only be called on failed results.");

        // Prefer the first error (most patterns treat the primary error as the one to surface)
        var error = result.Errors[0];

        if (error is not ApplicationError appError)
            // Fallback for unexpected error types (e.g. raw FluentResults Error)
            return TypedResults.Problem(
                error.Message ?? "An unexpected error occurred.",
                title: "Internal Server Error",
                statusCode: StatusCodes.Status500InternalServerError,
                extensions: new Dictionary<string, object?>
                {
                    ["code"] = error.GetType().Name
                });


        var problemDetails = new ProblemDetails
        {
            Title = appError.Title ?? "An error occurred",
            Detail = appError.Message,
            Status = GetErrorStatusCode(appError),
            Type = GetErrorProblemRfcType(appError),
            Extensions =
            {
                ["code"] = appError.Code
            }
        };

        if (result.Errors.Count > 1)
            problemDetails.Extensions["errors"] = result.Errors
                .Select(e => new { (e as ApplicationError)?.Code, e.Message })
                .ToList();

        return TypedResults.Problem(problemDetails);
    }

    private static int GetErrorStatusCode(ApplicationError error)
    {
        return error switch
        {
            UnauthorizedError => StatusCodes.Status401Unauthorized,
            ForbiddenError => StatusCodes.Status403Forbidden,
            NotFoundError => StatusCodes.Status404NotFound,
            ConflictError => StatusCodes.Status409Conflict,
            ValidationError => StatusCodes.Status422UnprocessableEntity,
            _ => StatusCodes.Status500InternalServerError
        };
    }

    private static string? GetErrorProblemRfcType(ApplicationError error)
    {
        // From table at https://datatracker.ietf.org/doc/html/rfc7231#section-6.1
        return error switch
        {
            UnauthorizedError => "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1",
            ForbiddenError => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.3",
            NotFoundError => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
            ConflictError => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8",
            ValidationError => "https://datatracker.ietf.org/doc/html/rfc9457#section-3-7",
            InternalError => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
            _ => null
        };
    }
}