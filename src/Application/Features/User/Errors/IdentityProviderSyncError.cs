using Application.Errors.ErrorTypes;

namespace Application.Features.User.Errors;

public class IdentityProviderSyncError()
    : InternalError("User.InternalSyncError", "Internal error", "An unexpected internal error occurred");