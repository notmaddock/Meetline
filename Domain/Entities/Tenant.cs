using Domain.Abstractions;

namespace Domain.Entities;

public class Tenant : AuditableEntity
{
    public Guid Id { get; init; }

    /// <summary>
    ///     The tenant's name
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    ///     The tenant identity provider's issuer URI, used for validating AND identifying which tenant a user belongs to
    /// </summary>
    public required string IssuerUri { get; set; }
}