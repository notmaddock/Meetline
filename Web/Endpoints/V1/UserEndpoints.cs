using Application.Features.User.GetOwnUserById;
using Application.Features.User.GetUserById;
using Web.Extensions;
using Web.Scopes;

namespace Web.Endpoints.V1;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/me", GetCurrentUser);
        app.MapGet("/{id:guid}", GetUserById);

        // TODO: Onboarding endpoint
    }

    private static async Task<IResult> GetCurrentUser(Mediator.Mediator mediator, CurrentUserScope scope)
    {
        var result = await mediator.Send(new GetOwnUserByIdQuery(scope.Id));

        return result.IsSuccess ? TypedResults.Ok(result.Value) : result.ToProblemDetails();
    }

    private static async Task<IResult> GetUserById(Mediator.Mediator mediator, Guid id)
    {
        var result = await mediator.Send(new GetUserByIdQuery(id));

        return result.IsSuccess ? TypedResults.Ok(result.Value) : result.ToProblemDetails();
    }
}