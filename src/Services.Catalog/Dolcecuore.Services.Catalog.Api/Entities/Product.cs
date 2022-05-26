using System;
using Dolcecuore.Domain.Entities;

namespace Dolcecuore.Services.Catalog.Api.Entities;

public class Product : AggregateRoot<Guid>
{
    public string Name { get; set; }
    public string Category { get; set; }
    public string Description { get; set; }
    public string ImagePath { get; set; }
    public decimal Price { get; set; }
}