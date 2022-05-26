using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Dolcecuore.Domain.Infrastructure.MessageBrokers;
using Dolcecuore.Domain.Repositories;
using Dolcecuore.Services.Catalog.Api.DTOs;
using Dolcecuore.Services.Catalog.Api.Entities;
using Microsoft.Extensions.Logging;

namespace Dolcecuore.Services.Catalog.Api.HostedServices;

public class PublishEventService
{
    private readonly ILogger<PublishEventService> _logger;
    private readonly IRepository<EventLog, long> _eventLogRepository;
    private readonly IMessageSender<AuditLogCreatedEvent> _auditLogCreatedEventSender;

    public PublishEventService(
        ILogger<PublishEventService> logger,
        IRepository<EventLog, long> eventLogRepository,
        IMessageSender<AuditLogCreatedEvent> auditLogCreatedEventSender)
    {
        _logger = logger;
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
                // TODO: Implement this
            }

            eventLog.Published = true;
            eventLog.UpdatedDateTime = DateTimeOffset.Now;
            await _eventLogRepository.UnitOfWork.SaveChangesAsync();
        }

        return events.Count;
    }
}