using System.Net;
using System.Text.Json;
using Application.Features.User.DeleteUserByExternalId;
using Application.Features.User.SyncUserFromIdentityProvider;
using Svix;

namespace Web.Endpoints.Webhooks;

public static class ClerkWebhooks
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };

    public static IEndpointRouteBuilder MapClerkWebhooks(this IEndpointRouteBuilder app)
    {
        app.MapPost("", RootWebhookHandler).AllowAnonymous();
        return app;
    }

    private static async Task<IResult> RootWebhookHandler(HttpRequest request, IConfiguration config,
        Mediator.Mediator mediator)
    {
        var secret = config["Clerk:WebhookSecret"];
        if (string.IsNullOrWhiteSpace(secret))
            return Results.Problem("Webhook secret not configured", statusCode: 500);

        using var reader = new StreamReader(request.Body);
        var payload = await reader.ReadToEndAsync();

        var headers = new WebHeaderCollection();
        foreach (var header in request.Headers) headers.Set(header.Key, header.Value!);

        try
        {
            var wh = new Webhook(secret);
            wh.Verify(payload, headers);
        }
        catch
        {
            return Results.BadRequest("Invalid signature");
        }

        var evt = JsonSerializer.Deserialize<ClerkWebhookEvent>(payload, SerializerOptions);
        if (evt is null) return Results.BadRequest("Invalid payload");

        var user = evt.Data.Deserialize<ClerkUser>(SerializerOptions);

        if (user is null) return Results.BadRequest("Null user");

        switch (evt.Type)
        {
            case "user.created":
            case "user.updated":
                await mediator.Send(new SyncUserFromIdentityProviderCommand(user.Id));
                break;

            case "user.deleted":
                await mediator.Send(new DeleteUserByExternalIdCommand(user.Id));
                break;

            // TODO case "user.deleted": > trigger soft-deletion
            default:
                return Results.Problem(statusCode: StatusCodes.Status418ImATeapot,
                    title: "Unsupported webhook event type");
        }

        return Results.Ok();
    }

    private record ClerkWebhookEvent(string Type, JsonElement Data);

    private record ClerkUser(string Id);
}