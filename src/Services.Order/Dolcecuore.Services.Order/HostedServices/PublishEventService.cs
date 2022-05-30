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

    public PublishEventService(
        IRepository<EventLog, long> eventLogRepository,
        IMessageSender<AuditLogCreatedEvent> auditLogCreatedEventSender)
    {
        _eventLogRepository = eventLogRepository;
        _auditLogCreatedEventSender = auditLogCreatedEventSender;
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
            else
            {
                // TODO: Handle other events
            }

            eventLog.Published = true;
            eventLog.UpdatedDateTime = DateTimeOffset.Now;
            await _eventLogRepository.UnitOfWork.SaveChangesAsync();
        }

        return events.Count;
    }
}
