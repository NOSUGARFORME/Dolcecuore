namespace Dolcecuore.Domain.Events;

public interface IDomainEvents
{
    Task DispatchAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default);
}