using System;
using Dolcecuore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dolcecuore.Services.Basket.Api.Entities;

public class EventLog : AggregateRoot<long>
{
    public string EventType { get; set; }

    public Guid TriggeredById { get; set; }

    public string ObjectId { get; set; }

    public string Message { get; set; }

    public bool Published { get; set; }
    
    internal sealed class Configuration : IEntityTypeConfiguration<EventLog>
    {
        public void Configure(EntityTypeBuilder<EventLog> builder)
        {
            builder.ToTable("EventLogs");
            builder
                .Property(el => el.Id)
                .UseIdentityColumn();
        }
    }
}