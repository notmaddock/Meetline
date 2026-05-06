using FluentValidation;
using Meetline.Modules.SharedKernel.Application.Validators;

namespace Meetline.Modules.Users.Application.Users.Commands.SyncUserFromIdentityProvider;

public class SyncUserFromIdentityProviderCommandValidator : AbstractValidator<SyncUserFromIdentityProviderCommand>
{
    public SyncUserFromIdentityProviderCommandValidator()
    {
        RuleFor(x => x.ExternalId).IsExternalId();
    }
}