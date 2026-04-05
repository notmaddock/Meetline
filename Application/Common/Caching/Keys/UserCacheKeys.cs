namespace Application.Common.Caching.Keys;

public static class UserCacheKeys
{
    public static string ById(Guid id)
    {
        return $"users:{id}";
    }

    public static string ByExternalId(string externalId)
    {
        return $"users:external:{externalId}";
    }

    public static string Exists(Guid id)
    {
        return $"users:exists:{id}";
    }

    public static string UsernameAvailability(string username)
    {
        return $"users:username-available:{username}";
    }
}