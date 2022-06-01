using Dolcecuore.Gateways.WebAPI.ConfigurationOptions;
using Ocelot.Configuration.File;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
var appSettings = new AppSettings();
builder.Configuration.Bind(appSettings);

builder.Services.AddOcelot();

builder.Services.PostConfigure<FileConfiguration>(configuration =>
{
    foreach (var route in appSettings.Ocelot.Routes.Select(x => x.Value))
    {
        var uri = new Uri(route.Downstream);

        foreach (var pathTemplate in route.UpstreamPathTemplates)
        {
            configuration.Routes.Add(new FileRoute
            {
                UpstreamPathTemplate = pathTemplate,
                DownstreamPathTemplate = pathTemplate,
                DownstreamScheme = uri.Scheme,
                DownstreamHostAndPorts = new List<FileHostAndPort>
                {
                    new() { Host = uri.Host, Port = uri.Port }
                }
            });
        }
    }
    
    foreach (var route in configuration.Routes)
    {
        if (string.IsNullOrWhiteSpace(route.DownstreamScheme))
        {
            route.DownstreamScheme = builder.Configuration["Ocelot:DefaultDownstreamScheme"];
        }

        if (string.IsNullOrWhiteSpace(route.DownstreamPathTemplate))
        {
            route.DownstreamPathTemplate = route.UpstreamPathTemplate;
        }
    }
});


var app = builder.Build();

await app.UseOcelot();

app.Run();