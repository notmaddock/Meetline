using Application.Features.Role.CreateRole;
using Application.Features.Role.DTOs.RoleResponse;
using Application.Features.Role.GetRoleById;
using Application.Features.Role.GetRoles;
using Microsoft.AspNetCore.Http.HttpResults;
using Web.Extensions;

namespace Web.Endpoints.V1;

public static class RoleEndpoints
{
    public static void MapRoleEndpoints(this IEndpointRouteBuilder app)
    {
        var roles = app.MapGroup("").WithTags("Roles");

        roles.MapGet("/", GetRoles)
            .WithName("GetRoles")
            .WithSummary("Get a list of available roles")
            .WithDescription("Returns a list of all available roles.");

        roles.MapGet("/{id:guid}", GetRoleById)
            .WithName("GetRoleById")
            .WithSummary("Get a role by its ID")
            .WithDescription("Returns a role's details given its ID.");

        roles.MapPost("/", CreateRole)
            .WithName("CreateRole")
            .WithSummary("Create a role")
            .WithDescription("Tries to create a role, doing permission checks in the middle.");
    }

    private static async Task<Results<Ok<ICollection<RoleResponse>>, ProblemHttpResult>> GetRoles(
        Mediator.Mediator mediator)
    {
        var result = await mediator.Send(new GetRolesQuery());

        return result.IsSuccess ? TypedResults.Ok(result.Value) : result.ToProblemHttpResult();
    }

    private static async Task<Results<Ok<RoleResponse>, NotFound, ProblemHttpResult>> GetRoleById(
        Mediator.Mediator mediator, Guid id)
    {
        var result = await mediator.Send(new GetRoleByIdQuery(id));

        return result.IsSuccess ? TypedResults.Ok(result.Value) : result.ToProblemHttpResult();
    }

    private static async Task<Results<Created<RoleResponse>, ForbidHttpResult, ProblemHttpResult>> CreateRole(
        Mediator.Mediator mediator, CreateRoleCommand command)
    {
        var result = await mediator.Send(command);

        return result.IsSuccess
            ? TypedResults.Created($"/api/roles/{result.Value}", result.Value)
            : result.ToProblemHttpResult();
    }
}