namespace Application.Services;

public interface IUnitOfWork {
    public Task<int> SaveChanges(CancellationToken token = default);
}
