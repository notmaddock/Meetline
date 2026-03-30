using FluentResults;
using Mediator;

namespace Application.Features.User.Exists;

public record ExistsQuery(Guid Id) : IQuery<Result<ExistsResponse>>;