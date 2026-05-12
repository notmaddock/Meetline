using Meetline.ServiceDefaults.Initialization;
using Microsoft.EntityFrameworkCore;

namespace Meetline.Modules.SharedKernel.Infrastructure.Initialization;

public class EfCoreInitializer<TContext>(TContext dbContext) : IDataInitializer
    where TContext : DbContext
{
    public async Task InitializeAsync(CancellationToken ct)
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () => { await dbContext.Database.MigrateAsync(ct); });
    }
}