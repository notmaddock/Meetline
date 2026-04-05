using Application.Repositories;
using FluentResults;
using Mediator;

namespace Application.Features.User.GetEmailAvailability;

public class GetEmailAvailabilityHandler(IUserRepository repository)
    : IQueryHandler<GetEmailAvailabilityQuery, Result<GetEmailAvailabilityResponse>>
{
    public async ValueTask<Result<GetEmailAvailabilityResponse>> Handle(GetEmailAvailabilityQuery query,
        CancellationToken cancellationToken)
    {
        var taken = await repository.ExistsByUsernameAsync(query.Email, cancellationToken);
        return Result.Ok(new GetEmailAvailabilityResponse(query.Email, !taken));
    }
}