using System;
using Dolcecuore.Domain.Entities;

namespace Dolcecuore.Services.Catalog.Api.Entities;

public class AuditLogEntry : AggregateRoot<Guid>
{
    public Guid UserId { get; set; }
    public string Action { get; set; }
    public string ObjectId { get; set; }
    public string Log { get; set; }
}