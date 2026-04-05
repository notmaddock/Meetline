using Application.Features.User.DTOs.UserResponse;
using Application.Features.User.Errors;
using Application.Repositories;
using FluentResults;
using Mediator;
using CreateUserRequestMapper = Application.Features.User.DTOs.CreateUserRequest.CreateUserRequestMapper;

namespace Application.Features.User.OnboardUser;

public class OnboardUserHandler(IUserRepository repository) : ICommandHandler<OnboardUserCommand, Result<UserResponse>>
{
    private readonly CreateUserRequestMapper _requestMapper = new();
    private readonly UserResponseMapper _responseMapper = new();

    public async ValueTask<Result<UserResponse>> Handle(OnboardUserCommand command, CancellationToken cancellationToken)
    {
        var existingUserId = await repository.GetUserIdFromExternalId(command.ExternalId, cancellationToken);
        if (existingUserId is not null) return Result.Fail(GenericUserErrors.UserAlreadyOnboardError());

        var user = _requestMapper.ToUser(command.Request, command.ExternalId);

        await repository.CreateAsync(user, cancellationToken);

        return Result.Ok(_responseMapper.ToResponse(user));
    }
}