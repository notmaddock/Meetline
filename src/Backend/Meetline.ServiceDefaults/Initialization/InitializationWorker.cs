using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Meetline.ServiceDefaults.Initialization;

public class InitializationWorker(
    IServiceProvider services,
    IHostApplicationLifetime lifetime,
    ILogger<InitializationWorker> logger) : BackgroundService
{
    public const string ActivitySourceName = "Migrations";
    private static readonly ActivitySource ActivitySource = new(ActivitySourceName);

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        using var activity = ActivitySource.StartActivity("Initializing", ActivityKind.Client);

        try
        {
            using var scope = services.CreateScope();
            var initializers = scope.ServiceProvider.GetServices<IDataInitializer>();

            foreach (var initializer in initializers)
            {
                logger.LogInformation("Running initializer: {Name}", initializer.GetType().Name);
                await initializer.InitializeAsync(ct);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Initialization failed");
            activity?.AddException(ex);
            throw;
        }

        lifetime.StopApplication();
    }
}