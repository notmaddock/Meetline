namespace Web.Endpoints.Webhooks;

public static class WebhookGroup
{
    public static IEndpointRouteBuilder MapWebhookEndpoints(this IEndpointRouteBuilder app)
    {
        var webhookGroup = app.MapGroup("");

        webhookGroup.MapGroup("clerk").MapClerkWebhooks();

        return webhookGroup;
    }
}