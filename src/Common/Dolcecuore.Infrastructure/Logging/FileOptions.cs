using Serilog.Events;

namespace Dolcecuore.Infrastructure.Logging;

public class FileOptions
{
    public LogEventLevel MinimumLogEventLevel { get; set; }
}