namespace Application.InternalServices;

public interface ITenantReadService
{
    Task<Guid?> GetTenantIdFromIssuerAsync(string issuer);
}