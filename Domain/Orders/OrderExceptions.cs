using Domain.Common.Exceptions;

namespace Domain.Orders;

public static class OrderExceptions {
    private static readonly ApplicationExceptionFactory factory = new(baseCode: nameof(Order));

    public static NotFoundException NotFound 
        => factory.NotFound(
            nameof(NotFound), 
            "Order not found");

    public static ConflictException CanReserveOnlyForOrderWaitingDeal
        => factory.Conflict(
            nameof(CanReserveOnlyForOrderWaitingDeal),
            "You can only reserve an order waiting for a deal");

    public static ConflictException CanConfirmPaymentOnlyForActiveOrder
        => factory.Conflict(
            nameof(CanConfirmPaymentOnlyForActiveOrder),
            "You can confirm the payment only for an active order");
}
