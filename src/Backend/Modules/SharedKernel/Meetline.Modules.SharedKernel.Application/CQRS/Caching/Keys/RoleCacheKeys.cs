namespace Meetline.Modules.SharedKernel.Application.CQRS.Caching.Keys;

public static class RoleCacheKeys
{
    public const string All = "roles:all";

    public static string ById(Guid id)
    {
        return $"roles:{id}";
    }
}