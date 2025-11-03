namespace BervProject.WebApi.Boilerplate.Services;

using Microsoft.Extensions.Logging;

/// <summary>
/// Cron Service
/// </summary>
public class CronService : ICronService
{
    private readonly ILogger<CronService> _logger;
    /// <summary>
    /// Cron Service Constructor with Dependency Injection
    /// </summary>
    /// <param name="loggerFactory"></param>
    public CronService(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<CronService>();
    }
    /// <summary>
    /// Hello World Cron
    /// </summary>
    public void HelloWorld()
    {
        _logger.LogDebug("Run Cron");
        _logger.LogInformation("Hello World Cron!");
        _logger.LogDebug("Finished Run Cron");
    }
}