using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dolcecuore.Application.Common.Query;
using Dolcecuore.Services.Catalog.Api.Entities;
using Dolcecuore.Services.Catalog.Api.Repositories;

namespace Dolcecuore.Services.Catalog.Api;

public record GetProductsQuery : IQuery<List<Product>>;

public class GetProductsQueryHandler : IQueryHandler<GetProductsQuery, List<Product>>
{
    private readonly IProductRepository _productRepository;

    public GetProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public Task<List<Product>> HandleAsync(GetProductsQuery query, CancellationToken cancellationToken = default)
    {
        return _productRepository.ToListAsync(_productRepository.GetAll());
    }
} 
