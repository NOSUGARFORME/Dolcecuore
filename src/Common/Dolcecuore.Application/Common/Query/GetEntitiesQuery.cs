using Dolcecuore.Domain.Entities;
using Dolcecuore.Domain.Repositories;

namespace Dolcecuore.Application.Common.Query;

public class GetEntitiesQuery<TEntity> : IQuery<List<TEntity>>
    where TEntity : AggregateRoot<Guid>
{
}

internal class GetEntitiesQueryHandler<TEntity> : IQueryHandler<GetEntitiesQuery<TEntity>, List<TEntity>>
    where TEntity : AggregateRoot<Guid>
{
    private readonly IRepository<TEntity, Guid> _repository;

    public GetEntitiesQueryHandler(IRepository<TEntity, Guid> repository)
    {
        _repository = repository;
    }

    public Task<List<TEntity>> HandleAsync(GetEntitiesQuery<TEntity> query, CancellationToken cancellationToken = default)
    {
        return _repository.ToListAsync(_repository.GetAll());
    }
}
