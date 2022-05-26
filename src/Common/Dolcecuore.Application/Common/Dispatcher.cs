using Dolcecuore.Application.Common.Commands;
using Dolcecuore.Application.Common.Query;

namespace Dolcecuore.Application.Common;

public class Dispatcher
{
    private readonly IServiceProvider _provider;
    
    public Dispatcher(IServiceProvider provider)
    {
        _provider = provider;
    }
    
    public async Task DispatchAsync(ICommand command, CancellationToken cancellationToken = default)
    {
        var type = typeof(ICommandHandler<>);
        Type[] typeArgs = { command.GetType() };
        var handlerType = type.MakeGenericType(typeArgs);

        dynamic handler = _provider.GetService(handlerType)!;
        await handler.HandleAsync((dynamic)command, cancellationToken);
    }
    
    public async Task<T> DispatchAsync<T>(IQuery<T> query, CancellationToken cancellationToken = default)
    {
        var type = typeof(IQueryHandler<,>);
        Type[] typeArgs = { query.GetType(), typeof(T) };
        var handlerType = type.MakeGenericType(typeArgs);

        dynamic handler = _provider.GetService(handlerType)!;
        Task<T> result = handler.HandleAsync((dynamic)query, cancellationToken);

        return await result;
    }
}