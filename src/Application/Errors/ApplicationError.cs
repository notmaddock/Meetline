using FluentResults;

namespace Application.Errors;

public abstract class ApplicationError : Error
{
    protected ApplicationError(string code, string title, string message)
        : base(message)
    {
        Code = code;
        Title = title;

        Metadata.Add("Code", code);
    }

    public string Code { get; }
    public string? Title { get; }
}