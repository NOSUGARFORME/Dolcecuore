using Dolcecuore.Application.Common.Services;
using Dolcecuore.Domain.Entities;

namespace Dolcecuore.Application.Common.Commands;

public class DeleteEntityCommand<TEntity> : ICommand
    where TEntity : AggregateRoot<Guid>
{
    public TEntity Entity { get; set; } = null!;
}

internal class DeleteEntityCommandHandler<TEntity> : ICommandHandler<DeleteEntityCommand<TEntity>>
    where TEntity : AggregateRoot<Guid>
{
    private readonly ICrudService<TEntity> _crudService;

    public DeleteEntityCommandHandler(ICrudService<TEntity> crudService)
    {
        _crudService = crudService;
    }

    public async Task HandleAsync(DeleteEntityCommand<TEntity> command, CancellationToken cancellationToken = default)
    {
        await _crudService.DeleteAsync(command.Entity, cancellationToken);
    }
}
