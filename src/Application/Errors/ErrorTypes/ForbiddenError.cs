namespace Application.Errors.ErrorTypes;

public abstract class ForbiddenError(string code, string title, string message)
    : ApplicationError(code, title, message);