using Domain.Common.Exceptions;

namespace Domain.Wallets;

public static class WalletExceptions {
    private static readonly ApplicationExceptionFactory factory = new(nameof(Wallet));

    public static ConflictException NotEnoughMoney
        => factory.Conflict(nameof(NotEnoughMoney), "Not enough money. Replanish wallet");
}
