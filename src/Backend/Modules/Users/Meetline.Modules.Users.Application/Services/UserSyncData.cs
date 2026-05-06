namespace Meetline.Modules.Users.Application.Services;

public record UserSyncData(
    string ExternalId,
    string Username,
    string Email,
    string? FirstName,
    string? LastName);