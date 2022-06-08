using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventLog;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

namespace Dolcecuore.Infrastructure.Logging;

public static class LoggingExtensions
{
    public static IWebHostBuilder UseLogger(this IWebHostBuilder builder,
        LoggingOptions logOptions)
    {
        builder.ConfigureLogging((context, logging) =>
        {
            logging.AddSerilog();
            
            var options = SetDefault(logOptions);
            
            if (options.EventLog is {IsEnabled: true})
            {
                logging.AddEventLog(new EventLogSettings
                {
                    LogName = options.EventLog.LogName,
                    SourceName = options.EventLog.SourceName,
                });
            }
            
            context.HostingEnvironment.UseLogger(options);
        });
        
        return builder;
    }
    
    private static void UseLogger(this IWebHostEnvironment env, LoggingOptions options)
    {
        var assemblyName = Assembly.GetEntryAssembly()?.GetName().Name;

        var logsPath = Path.Combine(env.ContentRootPath, "logs");
        Directory.CreateDirectory(logsPath);
        var loggerConfiguration = new LoggerConfiguration();
        
        loggerConfiguration = loggerConfiguration
            .MinimumLevel.Debug()
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithEnvironmentUserName()
            .Enrich.WithProperty("Assembly", assemblyName)
            .Enrich.WithProperty("Application", env.ApplicationName)
            .Enrich.WithProperty("EnvironmentName", env.EnvironmentName)
            .Enrich.WithProperty("ContentRootPath", env.ContentRootPath)
            .Enrich.WithProperty("WebRootPath", env.WebRootPath)
            .Enrich.WithExceptionDetails()
            .Filter.ByIncludingOnly(logEvent =>
                {
                    if (logEvent.Level < options.File.MinimumLogEventLevel &&
                        logEvent.Level < options.Elasticsearch.MinimumLogEventLevel) return false;
                    
                    var sourceContext = logEvent.Properties.ContainsKey("SourceContext")
                        ? logEvent.Properties["SourceContext"].ToString()
                        : null;

                    var logLevel = GetLogLevel(sourceContext, options);

                    return logEvent.Level >= logLevel;

                })
                .WriteTo.File(Path.Combine(logsPath, "log.txt"),
                    fileSizeLimitBytes: 10 * 1024 * 1024,
                    rollOnFileSizeLimit: true,
                    shared: true,
                    flushToDiskInterval: TimeSpan.FromSeconds(1),
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [{SourceContext}] [TraceId: {TraceId}] {Message:lj}{NewLine}{Exception}",
                    restrictedToMinimumLevel: options.File.MinimumLogEventLevel);

            if (options.Elasticsearch is {IsEnabled: true})
            {
                loggerConfiguration = loggerConfiguration
                    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(options.Elasticsearch.Host))
                    {
                        MinimumLogEventLevel = options.Elasticsearch.MinimumLogEventLevel,
                        AutoRegisterTemplate = true,
                        AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6,
                        IndexFormat = options.Elasticsearch.IndexFormat + "-{0:yyyy.MM.dd}",
                        InlineFields = true,
                    });
            }

            Log.Logger = loggerConfiguration.CreateLogger();
    }
    
    private static Serilog.Events.LogEventLevel GetLogLevel(string context, LoggingOptions options)
    {
        context = context.Replace("\"", string.Empty);
        var level = "Default";
        var matches = options.LogLevel.Keys.Where(key => context.StartsWith(key)).ToList();

        if (matches.Any())
        {
            level = matches.Max();
        }

        return (Serilog.Events.LogEventLevel)Enum.Parse(typeof(Serilog.Events.LogEventLevel), options.LogLevel[level], true);
    }
    
    private static LoggingOptions SetDefault(LoggingOptions options)
    {
        options ??= new LoggingOptions();

        options.LogLevel ??= new Dictionary<string, string>();

        if (!options.LogLevel.ContainsKey("Default"))
        {
            options.LogLevel["Default"] = "Warning";
        }

        options.File ??= new FileOptions
        {
            MinimumLogEventLevel = Serilog.Events.LogEventLevel.Warning,
        };

        options.Elasticsearch ??= new ElasticsearchOptions
        {
            IsEnabled = false,
            MinimumLogEventLevel = Serilog.Events.LogEventLevel.Warning,
        };

        options.EventLog ??= new EventLogOptions
        {
            IsEnabled = false,
        };
        
        return options;
    }
}