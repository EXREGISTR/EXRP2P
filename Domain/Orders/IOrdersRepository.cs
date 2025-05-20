
namespace Domain.Orders;

public interface IOrdersRepository {
    public Task<Order?> Find(Guid orderId, CancellationToken cancellationToken);
}
