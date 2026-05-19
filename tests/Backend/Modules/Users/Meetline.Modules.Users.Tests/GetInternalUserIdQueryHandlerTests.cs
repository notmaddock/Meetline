using Meetline.Modules.Users.Application.Users.Commands.SyncUserFromIdentityProvider;
using Meetline.Modules.Users.Application.Users.Queries.GetInternalUserId;
using Meetline.Modules.Users.Domain.Entities;
using Moq;
using Wolverine;

namespace Meetline.Modules.Users.Tests;

public sealed class GetInternalUserIdQueryHandlerTests : UsersDbTestBase
{
    [Fact(DisplayName = "Handle should return user ID directly when user already exists in the database")]
    public async Task Handle_ShouldReturnUserId_WhenUserAlreadyExists()
    {
        // Arrange
        const string externalId = "user_existing";
        var existingUser = new User
        {
            Id = Guid.NewGuid(),
            ExternalId = externalId,
            Username = "existing",
            Email = "existing@example.com"
        };
        Context.Users.Add(existingUser);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var query = new GetInternalUserIdQuery(externalId);
        var busMock = new Mock<IMessageBus>();

        // Act
        var result =
            await GetInternalUserIdQueryHandler.Handle(query, Context, busMock.Object,
                TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(existingUser.Id, result);
        busMock.Verify(b => b.InvokeAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact(DisplayName =
        "Handle should invoke SyncUserFromIdentityProviderCommand and return user ID when user does not exist")]
    public async Task Handle_ShouldInvokeSyncAndReturnUserId_WhenUserDoesNotExist()
    {
        // Arrange
        const string externalId = "user_to_sync";
        var syncedUserId = Guid.NewGuid();
        var query = new GetInternalUserIdQuery(externalId);

        var busMock = new Mock<IMessageBus>();
        busMock.Setup(b => b.InvokeAsync(It.Is<SyncUserFromIdentityProviderCommand>(c => c.ExternalId == externalId),
                It.IsAny<CancellationToken>()))
            .Returns(async () =>
            {
                Context.Users.Add(new User
                {
                    Id = syncedUserId,
                    ExternalId = externalId,
                    Username = "synced",
                    Email = "synced@example.com"
                });
                await Context.SaveChangesAsync();
            });

        // Act
        var result =
            await GetInternalUserIdQueryHandler.Handle(query, Context, busMock.Object,
                TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(syncedUserId, result);
        busMock.Verify(
            b => b.InvokeAsync(It.Is<SyncUserFromIdentityProviderCommand>(c => c.ExternalId == externalId),
                It.IsAny<CancellationToken>()), Times.Once);
    }
}