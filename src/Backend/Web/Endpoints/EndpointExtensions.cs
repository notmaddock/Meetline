using Asp.Versioning;
using Asp.Versioning.Conventions;
using Web.Endpoints.V1;
using Web.Endpoints.Webhooks;

namespace Web.Endpoints;

public static class EndpointExtensions
{
    public static void MapEndpoints(this IEndpointRouteBuilder app)
    {
        var root = app.MapGroup("");

        var versionSet = root.NewApiVersionSet()
            .HasApiVersions([new ApiVersion(1)])
            .Build();

        var api = root.MapGroup("/api").WithApiVersionSet(versionSet);

        api.MapGroup("/users").MapUserEndpoints();

        api.MapGroup("/webhooks").MapWebhookEndpoints();
    }
}