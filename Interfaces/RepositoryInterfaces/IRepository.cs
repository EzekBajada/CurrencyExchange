namespace CurrencyExchange.Interfaces.RepositoryInterfaces;

public interface IRepository<T>
{
    public Task<T?> GetOneById(int? id);

    public Task<IEnumerable<T>?> GetMultipleByFilter(Func<T, bool> filter);

    public Task AddOne(T entity);

    public Task SaveDbChanges();
}