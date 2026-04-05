using Application.Errors;
using FluentResults;
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

        var statusCode = appError.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

        var problemDetails = new ProblemDetails
        {
            Title = appError.Title ?? "An error occurred",
            Detail = appError.Message,
            Status = statusCode,
            Type = GetProblemType(appError.Type) // optional but recommended
        };

        problemDetails.Extensions["code"] = appError.Code;

        // Nice-to-have: include all errors if there are multiple (especially useful for Validation)
        if (result.Errors.Count > 1)
            problemDetails.Extensions["errors"] = result.Errors
                .Select(e => new { (e as ApplicationError)?.Code, e.Message })
                .ToList();

        return TypedResults.Problem(problemDetails);
    }

    private static string? GetProblemType(ErrorType errorType)
    {
        return errorType switch
        {
            ErrorType.Validation => "https://tools.ietf.org/html/rfc7231#section-6.5.1", // Bad Request
            ErrorType.NotFound => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            ErrorType.Conflict => "https://tools.ietf.org/html/rfc7231#section-6.5.8",
            ErrorType.Forbidden => "https://tools.ietf.org/html/rfc7231#section-6.5.3",
            _ => null
        };
    }
}