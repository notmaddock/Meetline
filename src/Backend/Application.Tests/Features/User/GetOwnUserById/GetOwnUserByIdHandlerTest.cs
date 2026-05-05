using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors.ErrorTypes;
using Application.Features.User.DTOs.UserResponse;
using Application.Features.User.GetOwnUserById;
using Application.Repositories;
using JetBrains.Annotations;
using Moq;
using Xunit;

namespace Application.Tests.Features.User.GetOwnUserById;

[TestSubject(typeof(GetOwnUserByIdHandler))]
public class GetOwnUserByIdHandlerTest
{
    private readonly GetOwnUserByIdHandler _handler;
    private readonly Mock<IUserRepository> _repositoryMock;

    public GetOwnUserByIdHandlerTest()
    {
        _repositoryMock = new Mock<IUserRepository>();
        _handler = new GetOwnUserByIdHandler(_repositoryMock.Object);
    }

    [Fact(DisplayName = "Base case; should return the user properly")]
    public async Task Handle_WhenUserExists_ShouldReturnUserResponse()
    {
        var userId = Guid.NewGuid();
        var query = new GetOwnUserByIdQuery(userId);

        var expectedUser = new Domain.Entities.User
        {
            Id = userId,
            ExternalId = null!,
            Username = null!,
            Email = null!
        };

        _repositoryMock
            .Setup(repo => repo.GetUserById(query.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedUser);

        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.IsType<UserResponse>(result.Value);
        Assert.Equal(userId, result.Value.Id);
    }

    [Fact(DisplayName =
        "Should return a result of the Forbidden variant when the user doesn't exist, meaning they're not onboard")]
    public async Task Handle_WhenUserDoesNotExist_ShouldReturnForbiddenError()
    {
        var userId = Guid.NewGuid();
        var query = new GetOwnUserByIdQuery(userId);

        _repositoryMock
            .Setup(repo => repo.GetUserById(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.User)null);

        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.True(result.IsFailed);
        Assert.True(result.HasError<NotFoundError>());
    }
}