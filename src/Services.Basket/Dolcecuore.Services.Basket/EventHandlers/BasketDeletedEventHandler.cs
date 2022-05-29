using Dolcecuore.Application.Common;
using Dolcecuore.CrossCuttingConcerns.ExtensionsMethods;
using Dolcecuore.Domain.Events;
using Dolcecuore.Domain.Repositories;
using Dolcecuore.Services.Basket.Commands;
using Dolcecuore.Services.Basket.Entities;

namespace Dolcecuore.Services.Basket.EventHandlers;

public class BasketDeletedEventHandler : IDomainEventHandler<EntityDeletedEvent<Basket.Entities.Basket>>
{
    private readonly Dispatcher _dispatcher;
    private readonly IRepository<EventLog, long> _eventLogRepository;

    public BasketDeletedEventHandler(Dispatcher dispatcher, IRepository<EventLog, long> eventLogRepository)
    {
        _dispatcher = dispatcher;
        _eventLogRepository = eventLogRepository;
    }

    public async Task HandleAsync(EntityDeletedEvent<Basket.Entities.Basket> domainEvent, CancellationToken cancellationToken = default)
    {
        await _dispatcher.DispatchAsync(new AddAuditLogEntryCommand(new AuditLogEntry
        {
            // UserId =
            CreatedDateTime = domainEvent.EventDateTime,
            Action = "DELETE_BASKET",
            ObjectId = domainEvent.Entity.Id.ToString(),
            Log = domainEvent.Entity.AsJsonString(),
        }), cancellationToken);

        await _eventLogRepository.AddOrUpdateAsync(new EventLog
        {
            EventType = "BASKET_DELETED",
            // TriggeredById =
            CreatedDateTime = domainEvent.EventDateTime,
            ObjectId = domainEvent.Entity.Id.ToString(),
            Message = domainEvent.Entity.AsJsonString(),
            Published = false,
        }, cancellationToken);

        await _eventLogRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
    }
}