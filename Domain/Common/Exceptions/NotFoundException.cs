namespace Domain.Common.Exceptions;

public class NotFoundException(string code, string message) : ApplicationExceptionBase(code, message) {
    public override ErrorType Type => ErrorType.NotFound;
}
