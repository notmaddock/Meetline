namespace Application.Features.User.DTOs.UserSyncData;

public record UserSyncData(
    string ExternalId,
    string Username,
    string Email,
    string? FirstName,
    string? LastName);