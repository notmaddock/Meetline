using Meetline.Modules.SharedKernel.Application.Errors.ErrorTypes;

namespace Meetline.Modules.Users.Application.Users.Errors;

public class IdentityProviderSyncError()
    : InternalError("User.InternalSyncError", "Internal error", "An unexpected internal error occurred");