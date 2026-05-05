namespace Application.Features.User.DTOs.CreateUserRequest;

public record CreateUserRequest
{
    public required string Username { get; init; }
    public required string Email { get; init; }

    public string? FirstName { get; init; }

    public string? LastName { get; init; }
}