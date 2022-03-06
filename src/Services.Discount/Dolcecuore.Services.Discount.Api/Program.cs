using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Dolcecuore.Services.Discount.Api.Extensions;

namespace Dolcecuore.Services.Discount.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            host.MigrateDatabase<Program>();
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
