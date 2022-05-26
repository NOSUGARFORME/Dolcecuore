using System.Linq.Expressions;
using Dolcecuore.Services.Order.Domain.Common;

namespace Dolcecuore.Services.Order.Application.Contracts.Persistence;

public interface IRepository<TEntity> where TEntity : EntityBase
{
    Task<List<TEntity>> GetAllAsync();
    Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate);

    Task<List<TEntity>> GetAsync(
        Expression<Func<TEntity, bool>>? predicate,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy,
        string? includeString = null,
        bool disableTracking = true);

    Task<List<TEntity>> GetAsync(
        Expression<Func<TEntity, bool>>? predicate,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy,
        List<Expression<Func<TEntity, object>>>? includes = null,
        bool disableTracking = true);

    ValueTask<TEntity?> GetByIdAsync(int id);
    Task<TEntity> AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
}
