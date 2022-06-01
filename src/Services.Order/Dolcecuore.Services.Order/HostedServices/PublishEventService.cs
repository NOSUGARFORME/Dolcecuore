using System.Text.Json;
using Dolcecuore.Domain.Infrastructure.MessageBrokers;
using Dolcecuore.Domain.Repositories;
using Dolcecuore.Services.Order.DTOs;
using Dolcecuore.Services.Order.Entities;

namespace Dolcecuore.Services.Order.HostedServices;

public class PublishEventService
{
    private readonly IRepository<EventLog, long> _eventLogRepository;
    private readonly IMessageSender<AuditLogCreatedEvent> _auditLogCreatedEventSender;
    private readonly IMessageSender<OrderCreatedEvent> _orderCreatedEventSender;

    public PublishEventService(
        IRepository<EventLog, long> eventLogRepository,
        IMessageSender<AuditLogCreatedEvent> auditLogCreatedEventSender,
        IMessageSender<OrderCreatedEvent> orderCreatedEventSender)
    {
        _eventLogRepository = eventLogRepository;
        _auditLogCreatedEventSender = auditLogCreatedEventSender;
        _orderCreatedEventSender = orderCreatedEventSender;
    }
    
    public async Task<int> PublishEvents()
    {
        var events = _eventLogRepository.GetAll()
            .Where(x => !x.Published)
            .OrderBy(x => x.CreatedDateTime)
            .Take(50)
            .ToList();

        foreach (var eventLog in events)
        {
            if (eventLog.EventType == "AUDIT_LOG_ENTRY_CREATED")
            {
                var logEntry = JsonSerializer.Deserialize<AuditLogEntry>(eventLog.Message);
                await _auditLogCreatedEventSender.SendAsync(new AuditLogCreatedEvent(logEntry));
            }
            else if (eventLog.EventType == "ORDER_CREATED")
            {
                var order = JsonSerializer.Deserialize<Entities.Order>(eventLog.Message);
                await _orderCreatedEventSender.SendAsync(new OrderCreatedEvent(order));
            }

            eventLog.Published = true;
            eventLog.UpdatedDateTime = DateTimeOffset.Now;
            await _eventLogRepository.UnitOfWork.SaveChangesAsync();
        }

        return events.Count;
    }
}
