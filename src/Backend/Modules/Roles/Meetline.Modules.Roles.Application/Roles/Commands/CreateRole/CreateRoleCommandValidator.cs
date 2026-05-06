using Application.Features.Role.DTOs.CreateRoleRequest;

namespace Application.Features.Role.CreateRole;

public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
        RuleFor(x => x.Request).SetValidator(new CreateRoleRequestValidator());
    }
}