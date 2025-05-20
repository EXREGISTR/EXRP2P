namespace Domain.Wallets;

public class Wallet {
    public Guid UserId { get; private set; }
    public Guid CurrencyId { get; private set; }

    public decimal Balance { get;private set; }
    public decimal LockedBalance { get; private set; }

    public decimal AvailableBalance => Balance - LockedBalance;

    private Wallet() { }

    public Wallet(Guid userId, Guid currencyId, decimal initialBalance) {
        if (initialBalance < 0) {
            throw new ArgumentException("Balance is should be greater or equals zero", nameof(initialBalance));
        }

        UserId = userId;
        CurrencyId = currencyId;
        Balance = initialBalance;
    }

    public void Lock(decimal amount) {
        if (AvailableBalance < amount) {
            throw new InvalidOperationException("Not enough money");
        }

        LockedBalance += amount;
    }

    public void Replenish(decimal amount) {
        Balance += amount;
    }

    public void Withdraw(decimal amount) {
        if (LockedBalance < amount) {
            throw new InvalidOperationException("Not enough money");
        }

        LockedBalance -= amount;
        Balance -= amount;
    }
}
