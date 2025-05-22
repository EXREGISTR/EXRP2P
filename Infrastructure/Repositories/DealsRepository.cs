using Domain.Deals;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal sealed class DealsRepository(ApplicationDbContext dbContext) : IDealsRepository {
    public Task<Deal?> Find(Guid orderId, CancellationToken token)
        => dbContext.Deals.FirstOrDefaultAsync(x => x.Id == orderId, token);
}
