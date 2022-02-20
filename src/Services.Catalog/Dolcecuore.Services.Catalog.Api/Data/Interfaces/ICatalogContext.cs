using Dolcecuore.Services.Catalog.Api.Entities;
using MongoDB.Driver;

namespace Dolcecuore.Services.Catalog.Api.Data.Interfaces
{
    public interface ICatalogContext
    {
        IMongoCollection<Product> Products { get; }
    }
}