using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Dolcecuore.Services.Order.Api.Extensions;

public static class HostExtensions
{
    public static IHost MigrateDatabase<TContext>(
        this IHost host,
        int retry = 0)
    where TContext : DbContext
    {
        using var scope = host.Services.CreateScope();
        
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<TContext>>();
        var context = services.GetService<TContext>();

        try
        {
            logger.LogInformation("Migrating database.");
            
            context.Database.Migrate();
            
            logger.LogInformation("Migrated database.");
        }
        catch (SqlException e)
        {
            logger.LogError(e, "An error occured while migrating the database.");

            if (retry < 50)
            {
                retry++;
                Thread.Sleep(2000);
                MigrateDatabase<TContext>(host, retry);
            }
        }

        return host;
    }
}
