using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace NET_Core_API_Boilerplate
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("init main");
                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                //NLog: catch setup errors
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                })
                .UseNLog();  // NLog: setup NLog for Dependency injection
    }
}
