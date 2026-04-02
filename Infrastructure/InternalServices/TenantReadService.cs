using Application.InternalServices;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.InternalServices;

public class TenantReadService(IDbContextFactory<ApplicationDbContext> context) : ITenantReadService
{
    public async Task<Guid?> GetTenantIdFromIssuerAsync(string issuer)
    {
        await using var ctx = await context.CreateDbContextAsync();

        return await ctx.Tenants
            .Where(t => t.IssuerUri == issuer)
            .Select(t => (Guid?)t.Id)
            .FirstOrDefaultAsync();
    }

    public async Task<Dictionary<string, Guid>> GetTenantIssuerMappingsAsync(CancellationToken ct = default)
    {
        await using var ctx = await context.CreateDbContextAsync(ct);

        return await ctx.Tenants
            .ToDictionaryAsync(t => t.IssuerUri, t => t.Id, ct);
    }
}