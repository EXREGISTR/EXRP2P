using Humanizer;

namespace Domain.Common.Exceptions;

public class ApplicationExceptionFactory(
    string? baseCode = null) {
    public NotFoundException NotFound(string code, string message)
        => new NotFoundException(CreateCode(code), message);

    public ConflictException Conflict(string code, string message)
        => new ConflictException(CreateCode(code), message);

    public ValidationException Validation(IDictionary<string, string[]> errors) {
        var errorsMap = errors
            .Select(x => new KeyValuePair<string, string[]>(
                CreateFieldName(x.Key), x.Value))
            .ToDictionary();

        return new ValidationException(errorsMap);
    }

    public ValidationException Validation(string fieldName, params string[] errors) {
        var errorsMap = new Dictionary<string, string[]> {
           { CreateFieldName(fieldName), errors },
        };

        return new ValidationException(errorsMap);
    }

    public ValidationException Validation(string fieldName, string error)
        => Validation(fieldName, [error]);

    private string CreateCode(string code) {
        string result = baseCode != null ? $"{baseCode}.{code}" : code;

        return result.Kebaberize();
    }

    private string CreateFieldName(string fieldName) {
        string result = baseCode != null ? $"{baseCode}.{fieldName}" : fieldName;

        return result.Kebaberize();
    }
}