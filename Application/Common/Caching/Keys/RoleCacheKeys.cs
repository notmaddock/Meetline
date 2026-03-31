namespace Application.Common.Caching.Keys;

public static class RoleCacheKeys
{
    public const string All = "roles:all";

    public static string ById(Guid id)
    {
        return $"roles:{id}";
    }
}