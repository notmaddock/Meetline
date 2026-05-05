namespace Web.Scopes;

public class CurrentUserScope
{
    public Guid Id { get; private set; }
    public string ExternalId { get; private set; } = string.Empty;

    internal void Populate(Guid id, string externalId)
    {
        Id = id;
        ExternalId = externalId;
    }
}