using Dolcecuore.Domain.Entities;
using Dolcecuore.Infrastructure.Persistence;

namespace Dolcecuore.Services.Order.Repositories;

public class Repository<T, TKey> : DbContextRepository<OrderDbContext, T, TKey>
    where T : AggregateRoot<TKey>
{
    public Repository(OrderDbContext dbContext) 
        : base(dbContext)
    {
    }
}