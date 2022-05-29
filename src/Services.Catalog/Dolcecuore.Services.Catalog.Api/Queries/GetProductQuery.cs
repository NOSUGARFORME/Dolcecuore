using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dolcecuore.Application.Common.Query;
using Dolcecuore.CrossCuttingConcerns.Exceptions;
using Dolcecuore.Services.Catalog.Api.Entities;
using Dolcecuore.Services.Catalog.Api.Repositories;

namespace Dolcecuore.Services.Catalog.Api.Queries;

public record GetProductQuery(Guid Id, bool ThrowNotFoundIfNull) : IQuery<Product>;

public class GetProductQueryHandler : IQueryHandler<GetProductQuery, Product>
{
    private readonly IProductRepository _productRepository;

    public GetProductQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Product> HandleAsync(GetProductQuery query, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.FirstOrDefaultAsync(_productRepository.GetAll().Where(x => x.Id == query.Id));

        if (query.ThrowNotFoundIfNull && product is null)
        {
            throw new NotFoundException($"Product {query.Id} not found.");
        }

        return product;
    }
}