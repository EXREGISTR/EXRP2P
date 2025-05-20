
namespace Domain.Deals;

public interface IDealsRepository {
    public Task<Deal?> Find(Guid orderId, CancellationToken cancellationToken);
}
