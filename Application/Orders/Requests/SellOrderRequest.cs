namespace Application.Orders.Requests;

public record CreateSellOrderRequest(
    Guid FromCurrencyId,
    Guid TargetCurrencyId,
    decimal Amount,
    decimal Price,
    string[] PaymentBanks);
