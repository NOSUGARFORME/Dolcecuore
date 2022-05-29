using Dolcecuore.Application.Common;
using Dolcecuore.CrossCuttingConcerns.ExtensionsMethods;
using Dolcecuore.Domain.Events;
using Dolcecuore.Domain.Repositories;
using Dolcecuore.Services.Basket.Commands;
using Dolcecuore.Services.Basket.Entities;

namespace Dolcecuore.Services.Basket.EventHandlers;

public class BasketCheckoutEventHandler : IDomainEventHandler<EntityCreatedEvent<Order>>
{
    private readonly Dispatcher _dispatcher;
    private readonly IRepository<EventLog, long> _eventLogRepository;

    public BasketCheckoutEventHandler(IRepository<EventLog, long> eventLogRepository, Dispatcher dispatcher)
    {
        _eventLogRepository = eventLogRepository;
        _dispatcher = dispatcher;
    }

    public async Task HandleAsync(EntityCreatedEvent<Order> domainEvent, CancellationToken cancellationToken = default)
    {
        await _dispatcher.DispatchAsync(new AddAuditLogEntryCommand(new AuditLogEntry
        {
            // UserId =
            CreatedDateTime = domainEvent.EventDateTime,
            Action = "CHECKOUT_BASKET",
            ObjectId = domainEvent.Entity.Id.ToString(),
            Log = domainEvent.Entity.AsJsonString(),
        }), cancellationToken);

        await _eventLogRepository.AddOrUpdateAsync(new EventLog
        {
            EventType = "BASKET_CHECKED",
            // TriggeredById =
            CreatedDateTime = domainEvent.EventDateTime,
            ObjectId = domainEvent.Entity.Id.ToString(),
            Message = domainEvent.Entity.AsJsonString(),
            Published = false,
        }, cancellationToken);

        await _eventLogRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
    }
}