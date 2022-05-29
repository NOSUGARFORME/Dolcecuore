using Dolcecuore.Application.Common;
using Dolcecuore.CrossCuttingConcerns.ExtensionsMethods;
using Dolcecuore.Domain.Events;
using Dolcecuore.Domain.Repositories;
using Dolcecuore.Services.Order.Commands;
using Dolcecuore.Services.Order.Entities;

namespace Dolcecuore.Services.Order.EventHandlers;

public class CheckoutOrderEventHandler : IDomainEventHandler<EntityCreatedEvent<Entities.Order>>
{
    private readonly Dispatcher _dispatcher;
    private readonly IRepository<EventLog, long> _eventLogRepository;

    public CheckoutOrderEventHandler(Dispatcher dispatcher, IRepository<EventLog, long> eventLogRepository)
    {
        _dispatcher = dispatcher;
        _eventLogRepository = eventLogRepository;
    }
    
    public async Task HandleAsync(EntityCreatedEvent<Entities.Order> domainEvent, CancellationToken cancellationToken = default)
    {
        await _dispatcher.DispatchAsync(new AddAuditLogEntryCommand(new AuditLogEntry
        {
            // UserId =
            CreatedDateTime = domainEvent.EventDateTime,
            Action = "CHECKED_ORDER",
            ObjectId = domainEvent.Entity.Id.ToString(),
            Log = domainEvent.Entity.AsJsonString()
        }), cancellationToken);

        await _eventLogRepository.AddOrUpdateAsync(new EventLog
        {
            EventType = "ORDER_CHECKED",
            // TriggeredById = 
            CreatedDateTime = domainEvent.EventDateTime,
            ObjectId = domainEvent.Entity.Id.ToString(),
            Message = domainEvent.Entity.AsJsonString(),
            Published = false,
        }, cancellationToken);

        await _eventLogRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
    }
}