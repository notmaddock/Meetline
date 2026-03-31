using FluentValidation;

namespace Application.Features.Role.CreateRole;

public class CreateRoleValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Role name is required.")
            .MaximumLength(100).WithMessage("Role name must not exceed 100 characters.");

        // Roles can't have position = 0 since it's reserved for the everyone role and we're
        // not doing snowflakes here so we don't have natural ID sorting in the database
        RuleFor(x => x.Position)
            .GreaterThanOrEqualTo(1).WithMessage("Position must be a non-negative integer over zero.");

        RuleFor(x => x.Permissions)
            .NotNull().WithMessage("Permissions are required.");
    }
}