using System.Collections.Concurrent;
using Application.InternalServices;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace Web.Tenancy;

public class OpenIdConfigurationService(ITenantReadService readService)
{
    private readonly ConcurrentDictionary<string, Guid> _issuerToId = new();
    private readonly ConcurrentDictionary<string, IConfigurationManager<OpenIdConnectConfiguration>> _managers = new();

    public async Task RefreshTenantMappingsAsync(CancellationToken ct = default)
    {
        var mappings = await readService.GetTenantIssuerMappingsAsync(ct);
        foreach (var (issuer, id) in mappings) _issuerToId.TryAdd(issuer, id);
    }

    internal Guid? GetTenantId(string issuer)
    {
        return _issuerToId.TryGetValue(issuer, out var cachedId) ? cachedId : null;
    }

    public async Task PreFetchConfigurationAsync(string issuer, CancellationToken ct = default)
    {
        var manager = GetManager(issuer);
        await manager.GetConfigurationAsync(ct);
    }

    private IConfigurationManager<OpenIdConnectConfiguration> GetManager(string issuer)
    {
        return _managers.GetOrAdd(issuer, key =>
        {
            var discoveryUrl = key.TrimEnd('/') + "/.well-known/openid-configuration";

            return new ConfigurationManager<OpenIdConnectConfiguration>(
                discoveryUrl,
                new OpenIdConnectConfigurationRetriever(),
                new HttpDocumentRetriever());
        });
    }

    public IEnumerable<SecurityKey> GetKeys(string issuer)
    {
        var manager = GetManager(issuer);

        // GetResult() is sadly required because the JwtBearer delegates are sync :(
        var config = manager.GetConfigurationAsync(CancellationToken.None).GetAwaiter().GetResult();
        return config.SigningKeys;
    }
}