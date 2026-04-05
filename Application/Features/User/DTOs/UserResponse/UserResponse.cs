namespace Application.Features.User.DTOs.UserResponse;

public record UserResponse(
    Guid Id,
    string Username,
    string Email,
    string? FirstName,
    string? LastName,
    DateTime CreatedAt,
    DateTime? UpdatedAt);