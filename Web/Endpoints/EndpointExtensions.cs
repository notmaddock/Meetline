using Asp.Versioning;
using Asp.Versioning.Conventions;

namespace Web.Endpoints;

public static class EndpointExtensions
{
    public static void MapEndpoints(this IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersions([new ApiVersion(1)])
            .Build();

        var api = app.MapGroup("/api").WithApiVersionSet(versionSet);
    }
}