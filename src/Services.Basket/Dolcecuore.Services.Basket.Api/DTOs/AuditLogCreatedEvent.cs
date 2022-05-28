using Dolcecuore.Services.Basket.Api.Entities;

namespace Dolcecuore.Services.Basket.Api.DTOs;

public class AuditLogCreatedEvent
{
    public AuditLogEntry AuditLog { get; set; }
}