using System.Threading;
using System.Threading.Tasks;
using Dolcecuore.Application.Common;
using Dolcecuore.CrossCuttingConcerns.ExtensionsMethods;
using Dolcecuore.Domain.Events;
using Dolcecuore.Domain.Repositories;
using Dolcecuore.Services.Catalog.Api.Commands;
using Dolcecuore.Services.Catalog.Api.Entities;

namespace Dolcecuore.Services.Catalog.Api.EventHandlers;

public class ProductUpdatedEventHandler : IDomainEventHandler<EntityUpdatedEvent<Product>>
{
    private readonly Dispatcher _dispatcher;
    private readonly IRepository<EventLog, long> _eventLogRepository;

    public ProductUpdatedEventHandler(Dispatcher dispatcher, IRepository<EventLog, long> eventLogRepository)
    {
        _dispatcher = dispatcher;
        _eventLogRepository = eventLogRepository;
    }

    public async Task HandleAsync(EntityUpdatedEvent<Product> domainEvent, CancellationToken cancellationToken = default)
    {
        await _dispatcher.DispatchAsync(new AddAuditLogEntryCommand(new AuditLogEntry
        {
            // UserId =
            CreatedDateTime = domainEvent.EventDateTime,
            Action = "PRODUCT_UPDATED",
            ObjectId = domainEvent.Entity.Id.ToString(),
            Log = domainEvent.Entity.AsJsonString()
        }), cancellationToken);

        await _eventLogRepository.AddOrUpdateAsync(new EventLog
        {
            EventType = "PRODUCT_UPDATED",
            // TriggeredById = 
            CreatedDateTime = domainEvent.EventDateTime,
            ObjectId = domainEvent.Entity.Id.ToString(),
            Message = domainEvent.Entity.AsJsonString(),
            Published = false,
        }, cancellationToken);

        await _eventLogRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
    }
}