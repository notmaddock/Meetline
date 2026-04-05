using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Application.Features.User.DTOs.UserGuidResponse;
using Application.Features.User.GetUserIdByExternalId;
using Application.Repositories;
using JetBrains.Annotations;
using Moq;
using Xunit;

namespace Application.Tests.Features.User.GetUserIdByExternalId;

[TestSubject(typeof(GetUserIdByExternalIdHandler))]
public class GetUserIdByExternalIdHandlerTest
{
    private readonly GetUserIdByExternalIdHandler _handler;
    private readonly Mock<IUserRepository> _repositoryMock;

    public GetUserIdByExternalIdHandlerTest()
    {
        _repositoryMock = new Mock<IUserRepository>();
        _handler = new GetUserIdByExternalIdHandler(_repositoryMock.Object);
    }

    [Fact(DisplayName = "Base case; should return the user's Guid properly")]
    public async Task Handle_WhenUserExists_ShouldReturnUserGuid()
    {
        var userId = Guid.NewGuid();
        const string externalId = "idp|external-id-1234";

        var query = new GetUserIdByExternalIdQuery(externalId);

        _repositoryMock
            .Setup(repo => repo.GetUserIdFromExternalId(query.ExternalId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(userId);

        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.IsType<UserGuidResponse>(result.Value);
        Assert.Equal(userId, result.Value.Id);
    }

    [Fact(DisplayName =
        "Should return a result of the NotFound variant when the user doesn't exist")]
    public async Task Handle_WhenUserDoesNotExist_ShouldReturnNotFoundError()
    {
        const string externalId = "idp|external-id-1234";

        var query = new GetUserIdByExternalIdQuery(externalId);

        _repositoryMock
            .Setup(repo => repo.GetUserIdFromExternalId(query.ExternalId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Guid?)null);

        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.True(result.IsFailed);
        var err = Assert.Single(result.Errors);
        Assert.IsType<ApplicationError>(err);
        Assert.Equal(ErrorType.NotFound, ((ApplicationError)err).Type);
    }
}