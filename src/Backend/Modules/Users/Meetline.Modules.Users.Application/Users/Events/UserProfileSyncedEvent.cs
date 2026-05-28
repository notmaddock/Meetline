namespace Meetline.Modules.Users.Application.Users.Events;

public record UserProfileSyncedEvent(Guid UserId, string ExternalId);