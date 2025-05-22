using Domain.Wallets;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal sealed class WalletsRepository(ApplicationDbContext dbContext) : IWalletsRepository {
    public Task<Wallet?> Find(Guid userId, Guid currencyId, CancellationToken token)
        => dbContext.Wallets.FirstOrDefaultAsync(x => x.UserId == userId && x.CurrencyId == currencyId, token);

    public Task<Wallet> FindForced(Guid userId, Guid currencyId, CancellationToken token)
        => dbContext.Wallets.FirstAsync(x => x.UserId == userId && x.CurrencyId == currencyId, token);

    public ValueTask Insert(Wallet wallet, CancellationToken token) {
        dbContext.Wallets.Add(wallet);
        return ValueTask.CompletedTask;
    }
}