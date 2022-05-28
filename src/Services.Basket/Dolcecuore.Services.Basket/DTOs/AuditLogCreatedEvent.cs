using Dolcecuore.Services.Basket.Entities;

namespace Dolcecuore.Services.Basket.DTOs;

public class AuditLogCreatedEvent
{
    public AuditLogEntry AuditLog { get; set; }
}