using System;
using Dolcecuore.Domain.Repositories;
using Dolcecuore.Services.Catalog.Api.Entities;

namespace Dolcecuore.Services.Catalog.Api.Repositories;

public interface IProductRepository : IRepository<Product, Guid>
{
}