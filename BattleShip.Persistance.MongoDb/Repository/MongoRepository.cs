
using BattleShip.Persistance.MongoDb.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BattleShip.Persistance.MongoDb.Repository;

internal class MongoRepository<T> : IRepository<T> where T : class
{
    private readonly BattleShipDbContext _context;
    private readonly DbSet<T> _dbSet;

    public MongoRepository(BattleShipDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<T> SingleOrDefault(System.Linq.Expressions.Expression<Func<T, bool>> filterOptions, CancellationToken cancellationToken)
    {
        var result = await _dbSet.SingleOrDefaultAsync(filterOptions, cancellationToken);

        return result;
    }

    public async Task<bool> AnyAsync(System.Linq.Expressions.Expression<Func<T, bool>> filterOptions, CancellationToken cancellationToken) =>
        await _dbSet.AnyAsync(filterOptions, cancellationToken);

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbSet.ToListAsync(cancellationToken);
    }

    public async Task<T> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        var result = await _dbSet.FindAsync(id, cancellationToken);

        if(result is null)
        {
            throw new InvalidOperationException($"Unable to get object of {typeof(T)} with Id {id}");
        }

        return result;
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Expression<Func<T, bool>> filterOptions, CancellationToken cancellationToken)
    {
        var toRemove = await _dbSet.Where(filterOptions).ToArrayAsync(cancellationToken);
        _dbSet.RemoveRange(toRemove);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAll(CancellationToken cancellationToken)
    {
        var toRemove = await _dbSet.ToArrayAsync(cancellationToken);
        _dbSet.RemoveRange(toRemove);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
