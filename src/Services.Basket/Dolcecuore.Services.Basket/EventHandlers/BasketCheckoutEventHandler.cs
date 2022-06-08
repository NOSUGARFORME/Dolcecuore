using Dolcecuore.Application.Common;
using Dolcecuore.CrossCuttingConcerns.ExtensionsMethods;
using Dolcecuore.Domain.Events;
using Dolcecuore.Domain.Repositories;
using Dolcecuore.Infrastructure.Identity;
using Dolcecuore.Services.Basket.Commands;
using Dolcecuore.Services.Basket.Entities;

namespace Dolcecuore.Services.Basket.EventHandlers;

public class BasketCheckoutEventHandler : IDomainEventHandler<EntityCreatedEvent<Order>>
{
    private readonly Dispatcher _dispatcher;
    private readonly IRepository<EventLog, long> _eventLogRepository;
    private readonly ICurrentUser _currentUser;

    public BasketCheckoutEventHandler(IRepository<EventLog, long> eventLogRepository, Dispatcher dispatcher,
        ICurrentUser currentUser)
    {
        _eventLogRepository = eventLogRepository;
        _dispatcher = dispatcher;
        _currentUser = currentUser;
    }

    public async Task HandleAsync(EntityCreatedEvent<Order> domainEvent, CancellationToken cancellationToken = default)
    {
        await _dispatcher.DispatchAsync(new AddAuditLogEntryCommand(new AuditLogEntry
        {
            UserId = _currentUser.UserId,
            CreatedDateTime = domainEvent.EventDateTime,
            Action = "CHECKOUT_BASKET",
            ObjectId = domainEvent.Entity.Id.ToString(),
            Log = domainEvent.Entity.AsJsonString(),
        }), cancellationToken);

        await _eventLogRepository.AddOrUpdateAsync(new EventLog
        {
            EventType = "BASKET_CHECKED",
            TriggeredById = _currentUser.UserId,
            CreatedDateTime = domainEvent.EventDateTime,
            ObjectId = domainEvent.Entity.Id.ToString(),
            Message = domainEvent.Entity.AsJsonString(),
            Published = false,
        }, cancellationToken);

        await _eventLogRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
    }
}