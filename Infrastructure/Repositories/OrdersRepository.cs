using Domain.Orders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal sealed class OrdersRepository(ApplicationDbContext dbContext) : IOrdersRepository {
    public Task<Order?> Find(Guid orderId, CancellationToken token)
        => dbContext.Orders.FirstOrDefaultAsync(x => x.Id == orderId, token);

    public ValueTask Insert(Order order, CancellationToken token) {
        dbContext.Orders.Add(order);
        return ValueTask.CompletedTask;
    }
}
