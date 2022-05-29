using System.Text.Json;
using Dolcecuore.Domain.Infrastructure.MessageBrokers;
using Dolcecuore.Domain.Repositories;
using Dolcecuore.Services.Basket.DTOs;
using Dolcecuore.Services.Basket.Entities;
using Dolcecuore.Services.Basket.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace Dolcecuore.Services.Basket.HostedServices;

public class PublishEventService
{
    private readonly ILogger<PublishEventService> _logger;
    private readonly IRepository<EventLog, long> _eventLogRepository;
    private readonly IMessageSender<AuditLogCreatedEvent> _auditLogCreatedEventSender;
    private readonly IMessageSender<BasketCheckedEvent> _basketCheckedEventSender;
    private readonly IBasketRepository _basketRepository;

    public PublishEventService(
        ILogger<PublishEventService> logger,
        IRepository<EventLog, long> eventLogRepository,
        IMessageSender<AuditLogCreatedEvent> auditLogCreatedEventSender,
        IBasketRepository basketRepository, IMessageSender<BasketCheckedEvent> basketCheckedEventSender)
    {
        _logger = logger;
        _eventLogRepository = eventLogRepository;
        _auditLogCreatedEventSender = auditLogCreatedEventSender;
        _basketRepository = basketRepository;
        _basketCheckedEventSender = basketCheckedEventSender;
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
            else if (eventLog.EventType == "BASKET_UPDATED")
            {
                var basket = JsonSerializer.Deserialize<Entities.Basket>(eventLog.Message);
                
            }
            else if (eventLog.EventType == "BASKET_DELETED")
            {
                var basket = JsonSerializer.Deserialize<Entities.Basket>(eventLog.Message);
                
            }
            else if (eventLog.EventType == "BASKET_CHECKED")
            {
                var order = JsonSerializer.Deserialize<Order>(eventLog.Message);
                await _basketCheckedEventSender.SendAsync(new BasketCheckedEvent(order));
            }

            eventLog.Published = true;
            eventLog.UpdatedDateTime = DateTimeOffset.Now;
            await _eventLogRepository.UnitOfWork.SaveChangesAsync();
        }

        return events.Count;
    }
}