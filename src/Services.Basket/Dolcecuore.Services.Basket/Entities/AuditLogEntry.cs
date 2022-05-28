using Dolcecuore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dolcecuore.Services.Basket.Entities;

public class AuditLogEntry : AggregateRoot<Guid>
{
    public Guid UserId { get; set; }
    public string Action { get; set; }
    public string ObjectId { get; set; }
    public string Log { get; set; }
    
    internal sealed class Configuration : IEntityTypeConfiguration<AuditLogEntry>
    {
        public void Configure(EntityTypeBuilder<AuditLogEntry> builder)
        {
            builder.ToTable("AuditLogEntries");
            builder
                .Property(ale => ale.Id)
                .HasDefaultValueSql("newsequentialid()");
        }
    }
}