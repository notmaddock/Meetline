using FluentValidation;

namespace Application.Features.User.DTOs.CreateUserRequest;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        Console.WriteLine("Validating create user req");
    }
}