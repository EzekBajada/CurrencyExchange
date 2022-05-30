namespace CurrencyExchange.Interfaces.RepositoryInterfaces;

public interface IRepository<T>
{
    public Task<T?> GetOneByIdAsync(int? id);

    public Task<IEnumerable<T>?> GetMultipleByFilterAsync(Func<T, bool>? filter);

    public Task AddOneAsync(T entity);

    public Task SaveDbChangesAsync();
}