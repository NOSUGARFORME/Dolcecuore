using System;
using Dolcecuore.Domain.Entities;

namespace Dolcecuore.Services.Catalog.Api.Entities;

public class EventLog : AggregateRoot<long>
{
    public string EventType { get; set; }

    public Guid TriggeredById { get; set; }

    public string ObjectId { get; set; }

    public string Message { get; set; }

    public bool Published { get; set; }
}