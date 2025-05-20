namespace Domain.Common.Exceptions;

public class ValidationException(IDictionary<string, string[]> errors) 
    : ApplicationExceptionBase(code: string.Empty, message: string.Empty) {
    public IDictionary<string, string[]> Errors { get; } = errors;

    public override ErrorType Type => ErrorType.Validation;
}
