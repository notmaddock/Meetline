using System.Text.Json;
using Web.Endpoints.Webhooks;

namespace Meetline.Modules.Users.Tests;

public class ClerkWebhookDeserializationTests
{
    [Fact(DisplayName = "Deserialization of user.created payload should parse all fields successfully")]
    public void Deserialize_UserCreatedPayload_ShouldParseFieldsSuccessfully()
    {
        const string json = """
                            {
                              "data": {
                                "id": "user_123",
                                "username": "johndoe",
                                "first_name": "John",
                                "last_name": "Doe",
                                "email_addresses": [
                                  {
                                    "email_address": "john.doe@example.com"
                                  }
                                ]
                              },
                              "type": "user.created"
                            }
                            """;

        var result = JsonSerializer.Deserialize<ClerkWebhookEndpoints.ClerkWebhookEvent>(json);

        Assert.NotNull(result);
        Assert.Equal("user.created", result.Type);
        Assert.NotNull(result.Data);
        Assert.Equal("user_123", result.Data.Id);
        Assert.Equal("johndoe", result.Data.Username);
        Assert.Equal("John", result.Data.FirstName);
        Assert.Equal("Doe", result.Data.LastName);
        Assert.NotNull(result.Data.EmailAddresses);
        var email = Assert.Single(result.Data.EmailAddresses);
        Assert.Equal("john.doe@example.com", email.EmailAddress);
    }

    [Fact(DisplayName = "Deserialization of user.deleted payload should succeed without email addresses")]
    public void Deserialize_UserDeletedPayload_ShouldSucceedWithoutEmailAddresses()
    {
        const string json = """
                            {
                              "data": {
                                "deleted": true,
                                "id": "user_123"
                              },
                              "type": "user.deleted"
                            }
                            """;

        var result = JsonSerializer.Deserialize<ClerkWebhookEndpoints.ClerkWebhookEvent>(json);

        Assert.NotNull(result);
        Assert.Equal("user.deleted", result.Type);
        Assert.NotNull(result.Data);
        Assert.Equal("user_123", result.Data.Id);
        Assert.Null(result.Data.Username);
        Assert.Null(result.Data.FirstName);
        Assert.Null(result.Data.LastName);
        Assert.Null(result.Data.EmailAddresses);
    }
}