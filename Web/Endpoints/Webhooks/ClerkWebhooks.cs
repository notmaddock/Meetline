namespace Web.Endpoints.Webhooks;

public static class ClerkWebhooks
{
    public static IEndpointRouteBuilder MapClerkWebhooks(this IEndpointRouteBuilder app)
    {
// TODO
        app.MapPost("", (HttpContext context) => { Console.WriteLine(context); }).AllowAnonymous();

        return app;
    }
}