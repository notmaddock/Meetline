namespace Domain.Abstractions;

public abstract class AuditableEntity
{
    /// <summary>
    ///     The creation date for the entity
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    ///     The last update date for the entity, if ever updated
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}