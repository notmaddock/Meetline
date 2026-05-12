namespace Meetline.ServiceDefaults.Initialization;

public interface IDataInitializer
{
    Task InitializeAsync(CancellationToken ct);
}