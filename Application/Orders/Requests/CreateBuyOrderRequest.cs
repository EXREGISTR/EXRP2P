namespace Application.Orders.Requests;

public record CreateBuyOrderRequest(
    Guid FromCurrencyId,
    Guid TargetCurrencyId,
    decimal Amount,
    decimal Price);
