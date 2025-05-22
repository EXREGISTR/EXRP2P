namespace Domain.Currencies;

public interface ICurrenciesRepository {
    public Task<int?> GetPrecision(Guid id, CancellationToken token);
}
