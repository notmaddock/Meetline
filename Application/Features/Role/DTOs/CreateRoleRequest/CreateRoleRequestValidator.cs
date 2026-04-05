using FluentValidation;

namespace Application.Features.Role.DTOs.CreateRoleRequest;

public class CreateRoleRequestValidator : AbstractValidator<CreateRoleRequest>
{
    public CreateRoleRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Role name cannot be empty");

        RuleFor(x => x.Position)
            .NotNull().WithMessage("Position is required")
            .Must(x => x >= 0).WithMessage("Position must be positive")
            .Must(x => x != 0).WithMessage("Position zero is reserved for the Everyone role");

        RuleFor(x => x.Hoist)
            .NotNull().WithMessage("Hoist is required");

        RuleFor(x => x.Permissions)
            .NotNull().WithMessage("Permissions must be defined");
    }
}