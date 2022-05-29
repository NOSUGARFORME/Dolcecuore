using Dolcecuore.Domain.Entities;
using Dolcecuore.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Dolcecuore.Infrastructure.Persistence;

public class DbContextRepository<TDbContext, TEntity, TKey> : IRepository<TEntity, TKey>
    where TEntity : AggregateRoot<TKey>
    where TDbContext : DbContext, IUnitOfWork
{
    private readonly TDbContext _dbContext;

    public DbContextRepository(TDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    protected DbSet<TEntity> DbSet => _dbContext.Set<TEntity>();
    
    public IUnitOfWork UnitOfWork => _dbContext;
    
    public async Task AddOrUpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        if (entity.Id.Equals(default(TKey)))
        {
            await AddAsync(entity, cancellationToken);
        }
        else
        {
            await UpdateAsync(entity, cancellationToken);
        }
    }
    
    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedDateTime = DateTimeOffset.Now;
        await DbSet.AddAsync(entity, cancellationToken);
    }

    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedDateTime = DateTimeOffset.Now;
        return Task.CompletedTask;
    }

    public void Delete(TEntity entity)
    {
        DbSet.Remove(entity);
    }

    public IQueryable<TEntity> GetAll()
    {
        return _dbContext.Set<TEntity>();
    }

    public Task<T1> FirstOrDefaultAsync<T1>(IQueryable<T1> query)
    {
        return query.FirstOrDefaultAsync();
    }

    public Task<T1> SingleOrDefaultAsync<T1>(IQueryable<T1> query)
    {
        return query.SingleOrDefaultAsync();
    }

    public Task<List<T1>> ToListAsync<T1>(IQueryable<T1> query)
    {
        return query.ToListAsync();
    }
}