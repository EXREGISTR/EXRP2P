using Domain.Common.Exceptions;

namespace Domain.Currencies;

public class Currency {
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Precision { get; set; }
    public bool IsFiat { get; set; }
}

public static class CurrencyExceptions {
    private static readonly ApplicationExceptionFactory factory = new(baseCode: nameof(Currency));

    public static NotFoundException SourceCurrencyNotFound
        => factory.NotFound(nameof(SourceCurrencyNotFound), "Source currency not found");

    public static NotFoundException TargetCurrencyNotFound
        => factory.NotFound(nameof(TargetCurrencyNotFound), "Target currency not found");
}