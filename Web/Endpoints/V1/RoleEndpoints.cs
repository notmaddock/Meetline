using Application.Features.Role.CreateRole;
using Application.Features.Role.GetRoleById;
using Application.Features.Role.GetRoles;
using Web.Extensions;

namespace Web.Endpoints.V1;

public static class RoleEndpoints
{
    public static void MapRoleEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/", GetRoles);
        app.MapGet("/{id:guid}", GetRoleById);
        app.MapPost("/", CreateRole);
    }

    private static async Task<IResult> GetRoles(Mediator.Mediator mediator)
    {
        var result = await mediator.Send(new GetRolesQuery());

        return result.IsSuccess ? TypedResults.Ok(result.Value) : result.ToProblemHttpResult();
    }

    private static async Task<IResult> GetRoleById(Mediator.Mediator mediator, Guid id)
    {
        var result = await mediator.Send(new GetRoleByIdQuery(id));

        return result.IsSuccess ? TypedResults.Ok(result.Value) : result.ToProblemHttpResult();
    }

    private static async Task<IResult> CreateRole(Mediator.Mediator mediator, CreateRoleCommand command)
    {
        var result = await mediator.Send(command);

        return result.IsSuccess
            ? TypedResults.Created($"/api/roles/{result.Value}", result.Value)
            : result.ToProblemHttpResult();
    }
}