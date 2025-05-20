namespace Domain.Common.Exceptions;

public class ConflictException(string code, string message) : ApplicationExceptionBase(code, message) {
    public override ErrorType Type => ErrorType.Conflict;
}