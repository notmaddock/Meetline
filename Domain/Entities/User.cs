using Domain.Abstractions;

namespace Domain.Entities;

public class User : AuditableTenantOwned
{
    public Guid Id { get; init; }

    public string ExternalId { get; init; } = null!;

    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}