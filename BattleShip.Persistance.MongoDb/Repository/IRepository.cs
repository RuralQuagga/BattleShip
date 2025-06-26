namespace BattleShip.Persistance.MongoDb.Repository;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);

    Task<T> GetByIdAsync(string id, CancellationToken cancellationToken);

    Task AddAsync(T entity, CancellationToken cancellationToken);

    Task UpdateAsync(T entity, CancellationToken cancellationToken);

    Task DeleteAsync(string id, CancellationToken cancellationToken);

    Task DeleteAsync(System.Linq.Expressions.Expression<Func<T, bool>> filterOptions, CancellationToken cancellationToken);

    Task<T> SingleOrDefault(System.Linq.Expressions.Expression<Func<T, bool>> filterOptions, CancellationToken cancellationToken);

    Task<bool> AnyAsync(System.Linq.Expressions.Expression<Func<T, bool>> filterOptions, CancellationToken cancellationToken);
}
