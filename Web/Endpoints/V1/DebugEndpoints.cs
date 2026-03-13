namespace Web.Endpoints.V1;

public static class DebugEndpoints
{
    public static void MapDebugV1Endpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/endpoints", (EndpointDataSource endpointDataSource) =>
        {
            var endpoints = endpointDataSource.Endpoints
                .OfType<RouteEndpoint>()
                .Select(e => new
                {
                    Route = e.RoutePattern.RawText,
                    e.DisplayName,
                    Methods = e.Metadata
                        .OfType<HttpMethodMetadata>()
                        .FirstOrDefault()?.HttpMethods
                });

            return Results.Ok(endpoints);
        });
    }
}