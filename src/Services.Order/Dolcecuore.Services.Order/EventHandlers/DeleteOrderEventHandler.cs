using Dolcecuore.Application.Common;
using Dolcecuore.CrossCuttingConcerns.ExtensionsMethods;
using Dolcecuore.Domain.Events;
using Dolcecuore.Domain.Repositories;
using Dolcecuore.Infrastructure.Identity;
using Dolcecuore.Services.Order.Commands;
using Dolcecuore.Services.Order.Entities;

namespace Dolcecuore.Services.Order.EventHandlers;

public class DeleteOrderEventHandler : IDomainEventHandler<EntityDeletedEvent<Entities.Order>>
{
    private readonly Dispatcher _dispatcher;
    private readonly IRepository<EventLog, long> _eventLogRepository;
    private readonly ICurrentUser _currentUser;

    public DeleteOrderEventHandler(Dispatcher dispatcher, IRepository<EventLog, long> eventLogRepository,
        ICurrentUser currentUser)
    {
        _dispatcher = dispatcher;
        _eventLogRepository = eventLogRepository;
        _currentUser = currentUser;
    }

    public async Task HandleAsync(EntityDeletedEvent<Entities.Order> domainEvent,
        CancellationToken cancellationToken = default)
    {
        await _dispatcher.DispatchAsync(new AddAuditLogEntryCommand(
            new AuditLogEntry
            {
                UserId = _currentUser.UserId,
                CreatedDateTime = domainEvent.EventDateTime,
                Action = "DELETED_ORDER",
                ObjectId = domainEvent.Entity.Id.ToString(),
                Log = domainEvent.Entity.AsJsonString(),
            }
        ), cancellationToken);

        await _eventLogRepository.AddOrUpdateAsync(new EventLog
        {
            EventType = "ORDER_DELETED",
            TriggeredById = _currentUser.UserId,
            CreatedDateTime = domainEvent.EventDateTime,
            ObjectId = domainEvent.Entity.Id.ToString(),
            Message = domainEvent.Entity.AsJsonString(),
            Published = false,
        }, cancellationToken);

        await _eventLogRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
    }
}