using Application.Features.User.DTOs.UserPublicResponse;
using Application.Features.User.Errors;
using Application.Repositories;
using FluentResults;
using Mediator;

namespace Application.Features.User.GetUserById;

public class GetUserByIdHandler(IUserRepository repository)
    : IQueryHandler<GetUserByIdQuery, Result<UserPublicResponse>>
{
    private readonly UserPublicResponseMapper _mapper = new();

    public async ValueTask<Result<UserPublicResponse>> Handle(GetUserByIdQuery query,
        CancellationToken cancellationToken)
    {
        var user = await repository.GetUserById(query.Id, cancellationToken);

        return user is null
            ? Result.Fail(new UserNotFoundError(query.Id))
            : Result.Ok(_mapper.ToResponse(user));
    }
}