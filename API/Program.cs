using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace API {

    public static class Program {

        public static void Main(string[] args) {
            ConfigureLogger();
            WebHost.CreateDefaultBuilder(args)
               .UseStartup<Startup>()
               .Build()
               .Run();
        }

        private static void ConfigureLogger() {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true).CreateLogger();
        }

    }

}