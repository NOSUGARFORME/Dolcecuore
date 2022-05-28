using System.Text.Json;
using Dolcecuore.Domain.Infrastructure.MessageBrokers;
using Dolcecuore.Domain.Repositories;
using Dolcecuore.Services.Basket.DTOs;
using Dolcecuore.Services.Basket.Entities;
using Dolcecuore.Services.Basket.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Dolcecuore.Services.Basket.HostedServices;

public class PublishEventService
{
    private readonly ILogger<PublishEventService> _logger;
    private readonly IRepository<EventLog, long> _eventLogRepository;
    private readonly IMessageSender<AuditLogCreatedEvent> _auditLogCreatedEventSender;
    private readonly IBasketRepository _basketRepository;

    public PublishEventService(
        ILogger<PublishEventService> logger,
        IRepository<EventLog, long> eventLogRepository,
        IMessageSender<AuditLogCreatedEvent> auditLogCreatedEventSender,
        IBasketRepository basketRepository)
    {
        _logger = logger;
        _eventLogRepository = eventLogRepository;
        _auditLogCreatedEventSender = auditLogCreatedEventSender;
        _basketRepository = basketRepository;
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
                try
                {
                    await _basketRepository.UpdateBasket(basket);
                }
                catch (RedisException e)
                {
                    _logger.LogError($"Redis exception: {e.Message}");
                    continue;
                }
            }

            eventLog.Published = true;
            eventLog.UpdatedDateTime = DateTimeOffset.Now;
            await _eventLogRepository.UnitOfWork.SaveChangesAsync();
        }

        return events.Count;
    }
}