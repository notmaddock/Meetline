using Meetline.Modules.SharedKernel.Application.Context;

namespace Web.Scopes;

public record WebProvidedCallerContext(Guid UserId) : ICallerContext;