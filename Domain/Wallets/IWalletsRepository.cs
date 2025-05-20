namespace Domain.Wallets;

public interface IWalletsRepository {
    public ValueTask Insert(Wallet wallet, CancellationToken token);

    public Task<Wallet> FindForced(Guid userId, Guid currencyId, CancellationToken token);
    public Task<Wallet?> Find(Guid userId, Guid currencyId, CancellationToken token);
}
