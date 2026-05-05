using FluentValidation;

namespace Application.Common.Validators;

public static class ExternalIdValidatorExtensions
{
    public static IRuleBuilderOptions<T, string> IsExternalId<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("External ID cannot be empty")
            .MinimumLength(30);
    }
}