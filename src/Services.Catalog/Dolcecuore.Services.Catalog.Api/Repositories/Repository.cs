using Dolcecuore.Domain.Entities;
using Dolcecuore.Infrastructure.Persistence;

namespace Dolcecuore.Services.Catalog.Api.Repositories;

public class Repository<T, TKey> : DbContextRepository<ProductDbContext, T, TKey>
    where T : AggregateRoot<TKey>
{
    public Repository(ProductDbContext dbContext)
        : base(dbContext)
    {
    }
}