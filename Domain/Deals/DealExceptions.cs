using Domain.Common.Exceptions;

namespace Domain.Deals;

public static class DealExceptions {
    private static readonly ApplicationExceptionFactory factory = new(baseCode: nameof(Deal));

    public static NotFoundException NotFound
        => factory.NotFound(
            nameof(NotFound), 
            "You didn't make a deal");

    public static ConflictException OnlySellerCanConfirmPayment
        => factory.Conflict(
            nameof(OnlySellerCanConfirmPayment), 
            "You aren't seller");
}
