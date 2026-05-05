using FluentValidation;

namespace Application.Features.User.DTOs.CreateUserRequest;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email must be a valid email address");

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required");
    }
}