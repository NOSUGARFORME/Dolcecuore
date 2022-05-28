using Dolcecuore.Domain.Entities;
using Dolcecuore.Infrastructure.Persistence;

namespace Dolcecuore.Services.Basket.Repositories;

public class Repository<T, TKey> : DbContextRepository<BasketEventDbContext, T, TKey>
    where T : AggregateRoot<TKey>
{
    public Repository(BasketEventDbContext dbContext)
        : base(dbContext)
    {
    }
}