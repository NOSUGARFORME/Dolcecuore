using System;
using Dolcecuore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dolcecuore.Services.Catalog.Api.Entities;

public class Product : AggregateRoot<Guid>
{
    public string Name { get; set; }
    public string Category { get; set; }
    public string Description { get; set; }
    public string ImagePath { get; set; }
    public decimal Price { get; set; }

    internal sealed class Configuration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");
            builder
                .Property(p => p.Id)
                .HasDefaultValueSql("newsequentialid()");
        }
    }
}