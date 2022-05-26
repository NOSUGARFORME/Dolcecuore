using System.Linq.Expressions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dolcecuore.Services.Order.Application.Behaviours;

public class UnhandledExceptionBehaviour<TRequest, TResponse> 
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<TRequest> _logger;
    
    public UnhandledExceptionBehaviour(ILogger<TRequest> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        try
        {
            return await next();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Application Request: Unhandled Exception for Request {Name} {@Request}", typeof(TRequest).Name, request);
            throw;
        }
    }
}