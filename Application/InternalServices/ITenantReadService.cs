namespace Application.InternalServices;

public interface ITenantReadService
{
    Task<Guid?> GetTenantIdFromIssuerAsync(string issuer);

    Task<Dictionary<string, Guid>> GetTenantIssuerMappingsAsync(CancellationToken ct = default);
}