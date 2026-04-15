using Application.Features.User.DTOs.UserResponse;
using Application.Features.User.Errors;
using Application.Repositories;
using FluentResults;
using Mediator;

namespace Application.Features.User.GetOwnUserById;

public class GetOwnUserByIdHandler(IUserRepository repository)
    : IQueryHandler<GetOwnUserByIdQuery, Result<UserResponse>>
{
    private readonly UserResponseMapper _mapper = new();

    public async ValueTask<Result<UserResponse>> Handle(GetOwnUserByIdQuery query,
        CancellationToken cancellationToken)
    {
        var user = await repository.GetUserById(query.Id, cancellationToken);

        return user is null
            ? Result.Fail(new UserNotFoundError(query.Id))
            : Result.Ok(_mapper.ToResponse(user));
    }
}