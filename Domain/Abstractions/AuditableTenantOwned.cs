namespace Domain.Abstractions;

public abstract class AuditableTenantOwned : AuditableEntity, ITenantOwned
{
    /// <summary>
    ///     The ID of the tenant that owns the child entity
    /// </summary>
    public Guid TenantId { get; set; }
}