using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors.ErrorTypes;
using Application.Features.User.Exists;
using Application.Repositories;
using JetBrains.Annotations;
using Moq;
using Xunit;

namespace Application.Tests.Features.User.Exists;

[TestSubject(typeof(ExistsHandler))]
public class ExistsHandlerTest
{
    private readonly ExistsHandler _handler;
    private readonly Mock<IUserRepository> _repositoryMock;

    public ExistsHandlerTest()
    {
        _repositoryMock = new Mock<IUserRepository>();
        _handler = new ExistsHandler(_repositoryMock.Object);
    }

    [Fact(DisplayName = "Should return an OK result when the user exists")]
    public async Task Handle_UserExists_ShouldReturnOkResult()
    {
        var id = Guid.Empty;

        _repositoryMock
            .Setup(repo => repo.ExistsAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var result = await _handler.Handle(new ExistsQuery(id), CancellationToken.None);

        Assert.True(result.IsSuccess);
    }

    [Fact(DisplayName = "Should return a failed result when the user does not exist with the appropriate domain error")]
    public async Task Handle_UserDoesNotExist_ShouldReturnFailedResultWithNotFoundError()
    {
        var id = Guid.Empty;

        _repositoryMock
            .Setup(repo => repo.ExistsAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var result = await _handler.Handle(new ExistsQuery(id), CancellationToken.None);

        Assert.True(result.IsFailed);
        Assert.True(result.HasError<NotFoundError>());
    }
}