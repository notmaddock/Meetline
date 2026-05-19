using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Meetline.Modules.Users.Application.Services;
using Meetline.Modules.Users.Application.Users.Commands.DeleteUser;
using Meetline.Modules.Users.Application.Users.Commands.UpsertUser;
using Microsoft.AspNetCore.Mvc;
using Svix;
using Wolverine;

namespace Web.Endpoints.Webhooks;

public static class ClerkWebhookEndpoints
{
    public static void MapClerkWebhookEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("clerk", HandleWebhook)
            .AllowAnonymous();
    }

    private static async Task<IResult> HandleWebhook(
        [FromHeader(Name = "svix-id")] string svixId,
        [FromHeader(Name = "svix-timestamp")] string svixTimestamp,
        [FromHeader(Name = "svix-signature")] string svixSignature,
        HttpRequest request,
        [FromServices] IConfiguration configuration,
        [FromServices] IMessageBus bus)
    {
        var secret = configuration["Clerk:WebhookSecret"];
        if (string.IsNullOrEmpty(secret)) return Results.InternalServerError("Webhook secret not configured.");

        using var reader = new StreamReader(request.Body);
        var payload = await reader.ReadToEndAsync();

        var svixHeaders = new WebHeaderCollection
        {
            { "svix-id", svixId },
            { "svix-timestamp", svixTimestamp },
            { "svix-signature", svixSignature }
        };

        try
        {
            var wh = new Webhook(secret);
            wh.Verify(payload, svixHeaders);
        }
        catch (Exception)
        {
            return Results.Unauthorized();
        }

        var webhookEvent = JsonSerializer.Deserialize<ClerkWebhookEvent>(payload);
        if (webhookEvent is null) return Results.BadRequest();

        if (webhookEvent.Type is "user.created" or "user.updated")
        {
            var userData = webhookEvent.Data;
            var email = userData.EmailAddresses?.FirstOrDefault()?.EmailAddress;

            if (string.IsNullOrEmpty(email)) return Results.BadRequest("No email address found in webhook data.");

            var syncData = new UserSyncData(
                userData.Id,
                userData.Username ?? string.Empty,
                email,
                userData.FirstName,
                userData.LastName);

            await bus.InvokeAsync(new UpsertUserCommand(syncData));
        }
        else if (webhookEvent.Type is "user.deleted")
        {
            var userData = webhookEvent.Data;
            await bus.InvokeAsync(new DeleteUserCommand(userData.Id));
        }

        return Results.Ok();
    }

    public record ClerkWebhookEvent(
        [property: JsonPropertyName("data")] ClerkUserData Data,
        [property: JsonPropertyName("type")] string Type);

    public record ClerkUserData(
        [property: JsonPropertyName("id")] string Id,
        [property: JsonPropertyName("username")]
        string? Username,
        [property: JsonPropertyName("first_name")]
        string? FirstName,
        [property: JsonPropertyName("last_name")]
        string? LastName,
        [property: JsonPropertyName("email_addresses")]
        List<ClerkEmailAddress>? EmailAddresses);

    public record ClerkEmailAddress(
        [property: JsonPropertyName("email_address")]
        string EmailAddress);
}