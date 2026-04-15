namespace Application.Errors.ErrorTypes;

public abstract class ValidationError(string code, string title, string message)
    : ApplicationError(code, title, message);