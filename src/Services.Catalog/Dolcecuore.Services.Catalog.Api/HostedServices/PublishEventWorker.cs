using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Dolcecuore.Services.Catalog.Api.HostedServices;

public class PublishEventWorker : BackgroundService
{
    private readonly IServiceProvider _services;
    private readonly ILogger<PublishEventWorker> _logger;

    public PublishEventWorker(IServiceProvider services, ILogger<PublishEventWorker> logger)
    {
        _services = services;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogDebug("PublishEventWorker is started.");
        await DoWork(stoppingToken);
    }
    
    private async Task DoWork(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogDebug("PublishEvent task doing background work.");

            try
            {
                int eventsCount;
                using (var scope = _services.CreateScope())
                {
                    var emailService = scope.ServiceProvider.GetRequiredService<PublishEventService>();

                    eventsCount = await emailService.PublishEvents();
                }

                if (eventsCount == 0)
                {
                    await Task.Delay(10000, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                await Task.Delay(10000, stoppingToken);
            }
        }

        _logger.LogDebug("PublishEventWorker background task is stopping.");
    }
}