using Application.Common.Validators;
using FluentValidation;

namespace Application.Features.User.SyncUserFromIdentityProvider;

public class SyncUserFromIdentityProviderCommandValidator : AbstractValidator<SyncUserFromIdentityProviderCommand>
{
    public SyncUserFromIdentityProviderCommandValidator()
    {
        RuleFor(x => x.ExternalId).IsExternalId();
    }
}