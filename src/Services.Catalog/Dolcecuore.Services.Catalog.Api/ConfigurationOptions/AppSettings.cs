using Dolcecuore.Infrastructure.MessageBrokers;

namespace Dolcecuore.Services.Catalog.Api.ConfigurationOptions;

public class AppSettings
{
    public ConnectionStrings ConnectionStrings { get; set; }
    
    public MessageBrokerOptions MessageBroker { get; set; }
}

public class ConnectionStrings
{
    public string Dolcecuore { get; set; }
    public string MigrationsAssembly { get; set; }
}