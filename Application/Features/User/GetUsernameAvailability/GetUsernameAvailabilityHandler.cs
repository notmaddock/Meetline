using Application.Repositories;
using FluentResults;
using Mediator;

namespace Application.Features.User.GetUsernameAvailability;

public class GetUsernameAvailabilityHandler(IUserRepository repository)
    : IQueryHandler<GetUsernameAvailabilityQuery, Result<GetUsernameAvailabilityResponse>>
{
    public async ValueTask<Result<GetUsernameAvailabilityResponse>> Handle(GetUsernameAvailabilityQuery query,
        CancellationToken cancellationToken)
    {
        var taken = await repository.ExistsByUsernameAsync(query.Username, cancellationToken);
        return Result.Ok(new GetUsernameAvailabilityResponse(query.Username, !taken));
    }
}