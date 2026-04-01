namespace Application.Scopes;

public interface ICurrentUserScope
{
    public Guid UserId { get; }

    public string ExternalUserId { get; }

    public Guid TenantId { get; }
}