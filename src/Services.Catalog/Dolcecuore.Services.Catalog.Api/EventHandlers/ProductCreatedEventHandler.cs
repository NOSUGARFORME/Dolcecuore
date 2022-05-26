using System.Threading;
using System.Threading.Tasks;
using Dolcecuore.Application.Common;
using Dolcecuore.CrossCuttingConcerns.ExtensionsMethods;
using Dolcecuore.Domain.Events;
using Dolcecuore.Domain.Repositories;
using Dolcecuore.Services.Catalog.Api.Commands;
using Dolcecuore.Services.Catalog.Api.Entities;

namespace Dolcecuore.Services.Catalog.Api.EventHandlers;

public class ProductCreatedEventHandler : IDomainEventHandler<EntityCreatedEvent<Product>>
{
    private readonly Dispatcher _dispatcher;
    private readonly IRepository<EventLog, long> _eventLogRepository;

    public ProductCreatedEventHandler(Dispatcher dispatcher, IRepository<EventLog, long> eventLogRepository)
    {
        _dispatcher = dispatcher;
        _eventLogRepository = eventLogRepository;
    }

    public async Task HandleAsync(EntityCreatedEvent<Product> domainEvent, CancellationToken cancellationToken = default)
    {
        await _dispatcher.DispatchAsync(new AddAuditLogEntryCommand(new AuditLogEntry
        {
            // UserId =
            CreatedDateTime = domainEvent.EventDateTime,
            Action = "CREATED_PRODUCT",
            ObjectId = domainEvent.Entity.Id.ToString(),
            Log = domainEvent.Entity.AsJsonString()
        }), cancellationToken);

        await _eventLogRepository.AddOrUpdateAsync(new EventLog
        {
            EventType = "PRODUCT_CREATED",
            // TriggeredById = 
            CreatedDateTime = domainEvent.EventDateTime,
            ObjectId = domainEvent.Entity.Id.ToString(),
            Message = domainEvent.Entity.AsJsonString(),
            Published = false,
        }, cancellationToken);

        await _eventLogRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
    }
}