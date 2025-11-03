namespace BervProject.WebApi.Boilerplate.Services;

using Microsoft.Extensions.Logging;

/// <summary>
/// Process Data Implementation
/// </summary>
public class ProcessData : IProcessData
{
    private readonly ILogger<ProcessData> _logger;
    /// <summary>
    /// Constructor with dependency injections
    /// </summary>
    /// <param name="logger"></param>
    public ProcessData(ILogger<ProcessData> logger)
    {
        _logger = logger;
    }
    /// <summary>
    /// Only logging
    /// </summary>
    /// <param name="message"></param>
    public void Process(string message)
    {
        _logger.LogDebug($"You get message: {message}");
        // TODO: another handler
    }
}