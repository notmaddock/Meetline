using Meetline.Modules.Users.Application.Users.Commands.DeleteUser;
using Meetline.Modules.Users.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Meetline.Modules.Users.Tests;

public sealed class DeleteUserCommandHandlerTests : UsersDbTestBase
{
    [Fact(DisplayName = "Handle should delete user when matching external ID exists")]
    public async Task Handle_ShouldDeleteUser_WhenMatchingExternalIdExists()
    {
        const string externalId = "user_123456";
        var user = new User
        {
            Id = Guid.NewGuid(),
            ExternalId = externalId,
            Username = "testuser",
            Email = "test@example.com"
        };
        Context.Users.Add(user);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var command = new DeleteUserCommand(externalId);

        await DeleteUserCommandHandler.Handle(command, Context, CancellationToken.None);

        var exists = await Context.Users.AnyAsync(u => u.ExternalId == externalId,
            TestContext.Current.CancellationToken);
        Assert.False(exists);
    }

    [Fact(DisplayName = "Handle should not delete other users when external ID does not match")]
    public async Task Handle_ShouldNotDeleteOtherUsers_WhenExternalIdDoesNotMatch()
    {
        const string targetExternalId = "user_target";
        const string otherExternalId = "user_other";

        var targetUser = new User
        {
            Id = Guid.NewGuid(),
            ExternalId = targetExternalId,
            Username = "target",
            Email = "target@example.com"
        };
        var otherUser = new User
        {
            Id = Guid.NewGuid(),
            ExternalId = otherExternalId,
            Username = "other",
            Email = "other@example.com"
        };

        Context.Users.AddRange(targetUser, otherUser);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var command = new DeleteUserCommand(targetExternalId);

        await DeleteUserCommandHandler.Handle(command, Context, CancellationToken.None);

        var targetExists = await Context.Users.AnyAsync(u => u.ExternalId == targetExternalId,
            TestContext.Current.CancellationToken);
        var otherExists = await Context.Users.AnyAsync(u => u.ExternalId == otherExternalId,
            TestContext.Current.CancellationToken);

        Assert.False(targetExists);
        Assert.True(otherExists);
    }
}