using Dolcecuore.Services.Catalog.Api.Entities;

namespace Dolcecuore.Services.Catalog.Api.DTOs;

public record AuditLogCreatedEvent(AuditLogEntry AuditLog);