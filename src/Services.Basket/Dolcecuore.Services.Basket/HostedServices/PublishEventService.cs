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
    private readonly IRepository<EventLog, long> _eventLogRepository;
    private readonly IMessageSender<AuditLogCreatedEvent> _auditLogCreatedEventSender;
    private readonly IMessageSender<BasketCheckedEvent> _basketCheckedEventSender;
    private readonly IMessageSender<BasketUpdatedEvent> _basketUpdatedEventSender;
    private readonly IMessageSender<BasketDeletedEvent> _basketDeletedEventSender;

    public PublishEventService(
        IRepository<EventLog, long> eventLogRepository,
        IMessageSender<AuditLogCreatedEvent> auditLogCreatedEventSender,
        IMessageSender<BasketCheckedEvent> basketCheckedEventSender,
        IMessageSender<BasketUpdatedEvent> basketUpdatedEventSender,
        IMessageSender<BasketDeletedEvent> basketDeletedEventSender)
    {
        _eventLogRepository = eventLogRepository;
        _auditLogCreatedEventSender = auditLogCreatedEventSender;
        _basketCheckedEventSender = basketCheckedEventSender;
        _basketUpdatedEventSender = basketUpdatedEventSender;
        _basketDeletedEventSender = basketDeletedEventSender;
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
            switch (eventLog.EventType)
            {
                case "AUDIT_LOG_ENTRY_CREATED":
                {
                    var logEntry = JsonSerializer.Deserialize<AuditLogEntry>(eventLog.Message);
                    await _auditLogCreatedEventSender.SendAsync(new AuditLogCreatedEvent(logEntry));
                    break;
                }
                case "BASKET_UPDATED":
                {
                    var basket = JsonSerializer.Deserialize<Entities.Basket>(eventLog.Message);
                    await _basketUpdatedEventSender.SendAsync(new BasketUpdatedEvent(basket));
                    break;
                }
                case "BASKET_DELETED":
                {
                    var basket = JsonSerializer.Deserialize<Entities.Basket>(eventLog.Message);
                    await _basketDeletedEventSender.SendAsync(new BasketDeletedEvent(basket));
                    break;
                }
                case "BASKET_CHECKED":
                {
                    var order = JsonSerializer.Deserialize<Order>(eventLog.Message);
                    await _basketCheckedEventSender.SendAsync(new BasketCheckedEvent(order));
                    break;
                }
            }

            eventLog.Published = true;
            eventLog.UpdatedDateTime = DateTimeOffset.Now;
            await _eventLogRepository.UnitOfWork.SaveChangesAsync();
        }

        return events.Count;
    }
}