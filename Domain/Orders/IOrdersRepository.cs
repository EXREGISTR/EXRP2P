﻿
namespace Domain.Orders;

public interface IOrdersRepository {
    public Task<Order?> Find(Guid orderId, CancellationToken token);
    public ValueTask Insert(Order order, CancellationToken token);
}
