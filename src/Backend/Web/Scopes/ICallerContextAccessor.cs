using Meetline.Modules.SharedKernel.Application.Context;

namespace Web.Scopes;

public interface ICallerContextAccessor
{
    ICallerContext? CallerContext { get; set; }
}

public class ScopedCallerContextAccessor : ICallerContextAccessor
{
    public ICallerContext? CallerContext { get; set; }
}