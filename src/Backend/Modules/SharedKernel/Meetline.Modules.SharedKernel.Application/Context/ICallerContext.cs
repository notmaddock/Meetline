namespace Meetline.Modules.SharedKernel.Application.Context;

public interface ICallerContext
{
    public Guid UserId { get; }
    public string UserExternalId { get; }
}