using Microsoft.Extensions.Logging;

namespace BervProject.WebApi.Boilerplate.Services
{
    public class CronService : ICronService
    {
        private readonly ILogger<CronService> _logger;
        public CronService(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<CronService>();
        }
        public void HelloWorld()
        {
            _logger.LogDebug("Run Cron");
            _logger.LogInformation("Hello World Cron!");
            _logger.LogDebug("Finished Run Cron");
        }
    }
}
