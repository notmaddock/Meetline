using Meetline.Modules.SharedKernel.Application.Context;
using Meetline.Modules.Users.Application.Users.Queries.GetCurrentUser;
using Meetline.Modules.Users.Domain.Entities;
using Moq;

namespace Meetline.Modules.Users.Tests;

public sealed class GetCurrentUserQueryHandlerTests : UsersDbTestBase
{
    [Fact(DisplayName = "Handle should return UserResponse when user exists and matches caller user ID")]
    public async Task Handle_ShouldReturnUserResponse_WhenUserExistsAndMatchesCaller()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var user = new User
        {
            Id = userId,
            ExternalId = "user_caller",
            Username = "calleruser",
            Email = "caller@example.com",
            FirstName = "John",
            LastName = "Doe"
        };

        Context.Users.Add(user);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var query = new GetCurrentUserQuery();
        var callerMock = new Mock<ICallerContext>();
        callerMock.Setup(c => c.UserId).Returns(userId);

        // Act
        var result = await GetCurrentUserQueryHandler.Handle(query, callerMock.Object, Context,
            TestContext.Current.CancellationToken);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userId, result.Id);
        Assert.Equal(user.Username, result.Username);
        Assert.Equal(user.Email, result.Email);
        Assert.Equal(user.FirstName, result.FirstName);
        Assert.Equal(user.LastName, result.LastName);
    }

    [Fact(DisplayName = "Handle should return null when user does not exist for caller user ID")]
    public async Task Handle_ShouldReturnNull_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var query = new GetCurrentUserQuery();
        var callerMock = new Mock<ICallerContext>();
        callerMock.Setup(c => c.UserId).Returns(userId);

        // Act
        var result = await GetCurrentUserQueryHandler.Handle(query, callerMock.Object, Context,
            TestContext.Current.CancellationToken);

        // Assert
        Assert.Null(result);
    }
}