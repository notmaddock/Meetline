using Application.Scopes;

namespace Web.Scopes;

public class CurrentUserScope : ICurrentUserScope
{
    public Guid UserId { get; private set; }
    public string ExternalUserId { get; private set; } = string.Empty;

    public Guid TenantId { get; private set; }

    internal void Populate(Guid id, string externalId, Guid tenantId)
    {
        UserId = id;
        ExternalUserId = externalId;
        TenantId = tenantId;
    }
}