using FluentValidation;

namespace Application.Features.User.OnboardUser;

public class OnboardUserCommandValidator : AbstractValidator<OnboardUserCommand>
{
    public OnboardUserCommandValidator()
    {
        Console.WriteLine("Validating onboard command");
    }
}