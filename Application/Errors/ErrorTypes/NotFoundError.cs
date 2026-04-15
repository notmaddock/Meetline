namespace Application.Errors.ErrorTypes;

public abstract class NotFoundError(string code, string title, string message) : ApplicationError(code, title, message);