namespace Domain.Abstractions;

public interface ITenantOwned
{
    public Guid TenantId { get; set; }
}