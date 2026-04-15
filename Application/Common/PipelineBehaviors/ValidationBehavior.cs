using Application.Errors.ErrorTypes;
using FluentResults;
using FluentValidation;
using Mediator;

namespace Application.Common.PipelineBehaviors;

public sealed class ValidationBehavior<TMessage, TResponse>(IEnumerable<IValidator<TMessage>> validators)
    : IPipelineBehavior<TMessage, TResponse>
    where TMessage : IMessage
    where TResponse : ResultBase, new()
{
    public async ValueTask<TResponse> Handle(TMessage message, MessageHandlerDelegate<TMessage, TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!validators.Any()) return await next(message, cancellationToken);

        var context = new ValidationContext<TMessage>(message);

        var validationFailures = validators
            .Select(validator => validator.Validate(context))
            .SelectMany(validationResult => validationResult.Errors)
            .Where(validationFailure => validationFailure is not null)
            .Select(failure => new GenericValidationError(
                failure.ErrorCode,
                failure.PropertyName,
                failure.ErrorMessage))
            .ToList();

        if (validationFailures.Count == 0) return await next(message, cancellationToken);

        var result = new TResponse();
        result.Reasons.AddRange(validationFailures);

        return result;
    }

    private class GenericValidationError(string code, string title, string message)
        : ValidationError(code, title, message);
}