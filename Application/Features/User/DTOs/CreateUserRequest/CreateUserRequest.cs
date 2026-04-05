namespace Application.Features.User.DTOs.CreateUserRequest;

public record CreateUserRequest(
    string Username,
    string Email,
    string? FirstName,
    string? LastName);