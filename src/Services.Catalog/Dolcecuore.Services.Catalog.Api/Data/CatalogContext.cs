using Dolcecuore.Services.Catalog.Api.Data.Interfaces;
using Dolcecuore.Services.Catalog.Api.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Dolcecuore.Services.Catalog.Api.Data
{
    public class CatalogContext: ICatalogContext
    {
        public CatalogContext(IConfiguration configuration)
        {            
            var client = new MongoClient(configuration["Mongo:ConnectionString"]);
            var database = client.GetDatabase(configuration["Mongo:Database"]);

            Products = database.GetCollection<Product>(configuration["Mongo:Collection"]);
        }

        public IMongoCollection<Product> Products { get; }

    }
}