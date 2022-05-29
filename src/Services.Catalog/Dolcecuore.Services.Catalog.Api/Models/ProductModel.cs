using System;

namespace Dolcecuore.Services.Catalog.Api.Models;

public class ProductModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public string Description { get; set; }
    public string ImagePath { get; set; }
    public decimal Price { get; set; }
}