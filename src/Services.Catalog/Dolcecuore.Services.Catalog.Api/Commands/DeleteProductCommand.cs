using System.Threading;
using System.Threading.Tasks;
using Dolcecuore.Application.Common.Commands;
using Dolcecuore.Application.Common.Services;
using Dolcecuore.Services.Catalog.Api.Entities;

namespace Dolcecuore.Services.Catalog.Api.Commands;

public record DeleteProductCommand(Product Product) : ICommand;

public class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand>
{
    private readonly ICrudService<Entities.Product> _productService;

    public DeleteProductCommandHandler(ICrudService<Entities.Product> productService)
    {
        _productService = productService;
    }

    public async Task HandleAsync(DeleteProductCommand command, CancellationToken cancellationToken = default)
    {
        await _productService.DeleteAsync(command.Product, cancellationToken);
    }
}
