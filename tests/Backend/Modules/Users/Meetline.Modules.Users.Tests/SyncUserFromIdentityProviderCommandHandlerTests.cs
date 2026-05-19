using FluentResults;
using Meetline.Modules.Users.Application.Services;
using Meetline.Modules.Users.Application.Users.Commands.SyncUserFromIdentityProvider;
using Meetline.Modules.Users.Application.Users.Commands.UpsertUser;
using Moq;
using Wolverine;

namespace Meetline.Modules.Users.Tests;

public sealed class SyncUserFromIdentityProviderCommandHandlerTests
{
    [Fact(DisplayName = "Handle should invoke UpsertUserCommand when identity provider sync succeeds")]
    public async Task Handle_ShouldInvokeUpsertUserCommand_WhenSyncSucceeds()
    {
        // Arrange
        const string externalId = "user_idp_123";
        var syncData = new UserSyncData(
            externalId,
            "idpuser",
            "idp@example.com",
            "FirstName",
            "LastName"
        );

        var identityProviderMock = new Mock<IIdentityProviderClientService>();
        identityProviderMock
            .Setup(ip => ip.GetUser(externalId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(syncData));

        var busMock = new Mock<IMessageBus>();

        var command = new SyncUserFromIdentityProviderCommand(externalId);

        // Act
        await SyncUserFromIdentityProviderCommandHandler.Handle(
            command,
            identityProviderMock.Object,
            busMock.Object,
            CancellationToken.None);

        // Assert
        busMock.Verify(b => b.InvokeAsync(
                It.Is<UpsertUserCommand>(c => c.Data == syncData),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact(DisplayName = "Handle should throw InvalidOperationException when identity provider sync fails")]
    public async Task Handle_ShouldThrowException_WhenSyncFails()
    {
        // Arrange
        const string externalId = "user_idp_123";

        var identityProviderMock = new Mock<IIdentityProviderClientService>();
        identityProviderMock
            .Setup(ip => ip.GetUser(externalId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail<UserSyncData>("User not found on identity provider"));

        var busMock = new Mock<IMessageBus>();

        var command = new SyncUserFromIdentityProviderCommand(externalId);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            SyncUserFromIdentityProviderCommandHandler.Handle(
                command,
                identityProviderMock.Object,
                busMock.Object,
                CancellationToken.None));

        Assert.Contains("Failed to sync user from identity provider", exception.Message);
        busMock.Verify(b => b.InvokeAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}