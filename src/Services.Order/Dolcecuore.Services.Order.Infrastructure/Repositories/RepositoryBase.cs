using System.Linq.Expressions;
using Dolcecuore.Services.Order.Application.Contracts.Persistence;
using Dolcecuore.Services.Order.Domain.Common;
using Dolcecuore.Services.Order.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Dolcecuore.Services.Order.Infrastructure.Repositories;

public class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : EntityBase
{
    protected readonly OrderContext Context;

    protected RepositoryBase(OrderContext context)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public Task<List<TEntity>> GetAllAsync()
        => Context.Set<TEntity>().ToListAsync();
    

    public Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate)
        => Context.Set<TEntity>().Where(predicate).ToListAsync();
    

    public Task<List<TEntity>> GetAsync(
        Expression<Func<TEntity, bool>>? predicate,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy,
        string? includeString = null,
        bool disableTracking = true)
    {
        IQueryable<TEntity> query = Context.Set<TEntity>();
        if (disableTracking)
        {
            query = query.AsNoTracking();
        }

        if (!string.IsNullOrWhiteSpace(includeString))
        {
            query = query.Include(includeString);
        }

        if (predicate is not null)
        {
            query = query.Where(predicate);
        }

        if (orderBy is not null)
        {
            return orderBy(query).ToListAsync();
        }

        return query.ToListAsync();
    }

    public Task<List<TEntity>> GetAsync(
        Expression<Func<TEntity, bool>>? predicate,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy,
        List<Expression<Func<TEntity, object>>>? includes = null,
        bool disableTracking = true)
    {
        IQueryable<TEntity> query = Context.Set<TEntity>();
        if (disableTracking)
        {
            query = query.AsNoTracking();
        }

        if (includes is not null)
        {
            query = includes.Aggregate(query, (current, include) => current.Include(include));
        }

        if (predicate is not null)
        { 
            query = query.Where(predicate);
        }

        if (orderBy is not null)
        {
            return orderBy(query).ToListAsync();
        }

        return query.ToListAsync();
    }


    public ValueTask<TEntity?> GetByIdAsync(int id)
        => Context.Set<TEntity>().FindAsync(id);
    

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        Context.Set<TEntity>().Add(entity);
        await Context.SaveChangesAsync();
        
        return entity;
    }

    public Task UpdateAsync(TEntity entity)
    {
        Context.Entry(entity).State = EntityState.Modified;
        return Context.SaveChangesAsync();
    }

    public Task DeleteAsync(TEntity entity)
    {
        Context.Set<TEntity>().Remove(entity);
        return Context.SaveChangesAsync();
    }
}
