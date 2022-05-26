using System.Threading;
using System.Threading.Tasks;
using Dolcecuore.Application.Common.Commands;
using Dolcecuore.Application.Common.Services;
using Dolcecuore.Services.Catalog.Api.Entities;

namespace Dolcecuore.Services.Catalog.Api.Commands;

public record AddUpdateProductCommand(Product Product) : ICommand;

public class AddUpdateProductCommandHandler : ICommandHandler<AddUpdateProductCommand>
{
    private readonly ICrudService<Product> _productService;

    public AddUpdateProductCommandHandler(ICrudService<Product> productService)
    {
        _productService = productService;
    }

    public async Task HandleAsync(AddUpdateProductCommand command, CancellationToken cancellationToken = default)
    {
        await _productService.AddOrUpdateAsync(command.Product, cancellationToken);
    }
}