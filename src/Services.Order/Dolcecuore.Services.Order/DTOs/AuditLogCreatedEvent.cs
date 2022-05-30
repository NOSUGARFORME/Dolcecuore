using Dolcecuore.Services.Order.Entities;

namespace Dolcecuore.Services.Order.DTOs;

public record AuditLogCreatedEvent(AuditLogEntry AuditLog);
