using Meetline.Modules.Common.Domain.Abstractions;

namespace Meetline.Modules.Users.Domain.Entities;

public class User : AuditableEntity
{
    public Guid Id { get; init; }

    public required string ExternalId { get; init; }

    public required string Username { get; set; }
    public required string Email { get; set; }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}