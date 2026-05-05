using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Web.Configs;

public class BadHttpRequestExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var isBadRequest = exception is BadHttpRequestException;
        var jsonException = exception as JsonException ?? exception.InnerException as JsonException;

        if (!isBadRequest && jsonException is null)
            return false;

        var detailMessage = "Invalid request payload.";

        if (jsonException is not null)
            detailMessage = jsonException.Message;
        else if (isBadRequest) detailMessage = exception.Message;

        var problem = new ProblemDetails
        {
            Title = "Bad Request",
            Status = StatusCodes.Status400BadRequest,
            Detail = detailMessage,
            Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
            Instance = httpContext.Request.Path
        };

        httpContext.Response.StatusCode = problem.Status.Value;
        httpContext.Response.ContentType = "application/problem+json";

        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);

        return true;
    }
}