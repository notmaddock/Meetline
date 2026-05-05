namespace Application.Errors.ErrorTypes;

public abstract class UnauthorizedError(string code, string title, string message)
    : ApplicationError(code, title, message);