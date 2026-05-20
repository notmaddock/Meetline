using Meetline.Modules.Users.Application.Repositories;
using Meetline.Modules.Users.Application.Services;
using Meetline.Modules.Users.Application.Users.Commands.UpsertUser;
using Meetline.Modules.Users.Domain.Entities;
using Moq;

namespace Meetline.Modules.Users.Tests;

public sealed class UpsertUserCommandHandlerTests
{
    [Fact(DisplayName = "Handle should map sync data to User and upsert in repository")]
    public async Task Handle_ShouldMapSyncDataAndCallUpsert_InRepository()
    {
        // Arrange
        var syncData = new UserSyncData(
            "external_id_123",
            "upsertuser",
            "upsert@example.com",
            "First",
            "Last"
        );

        var command = new UpsertUserCommand(syncData);
        var repositoryMock = new Mock<IUserRepository>();

        User? upsertedUser = null;
        repositoryMock
            .Setup(r => r.UpsertByExternalIdAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .Callback<User, CancellationToken>((u, _) => upsertedUser = u)
            .ReturnsAsync((User u, CancellationToken _) => u);

        // Act
#pragma warning disable EXTEXP0018
        var cache = new MockHybridCache();
#pragma warning restore EXTEXP0018
        await UpsertUserCommandHandler.Handle(command, repositoryMock.Object, cache, CancellationToken.None);

        // Assert
        repositoryMock.Verify(r => r.UpsertByExternalIdAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()),
            Times.Once);
        Assert.NotNull(upsertedUser);
        Assert.Equal(syncData.ExternalId, upsertedUser.ExternalId);
        Assert.Equal(syncData.Username, upsertedUser.Username);
        Assert.Equal(syncData.Email, upsertedUser.Email);
        Assert.Equal(syncData.FirstName, upsertedUser.FirstName);
        Assert.Equal(syncData.LastName, upsertedUser.LastName);
    }
}