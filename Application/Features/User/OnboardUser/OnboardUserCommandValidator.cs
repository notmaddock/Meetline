using Application.Common.Validators;
using Application.Features.User.DTOs.CreateUserRequest;
using FluentValidation;

namespace Application.Features.User.OnboardUser;

public class OnboardUserCommandValidator : AbstractValidator<OnboardUserCommand>
{
    public OnboardUserCommandValidator()
    {
        RuleFor(x => x.ExternalId).IsExternalId();
        RuleFor(x => x.Request).SetValidator(new CreateUserRequestValidator());
    }
}