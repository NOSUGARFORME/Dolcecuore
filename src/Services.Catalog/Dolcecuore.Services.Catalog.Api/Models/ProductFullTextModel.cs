using System;

namespace Dolcecuore.Services.Catalog.Api.Models;

public class ProductFullTextModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}