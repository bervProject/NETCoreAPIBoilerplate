using Microsoft.Extensions.Logging;

namespace BervProject.WebApi.Boilerplate.Services
{
    public class ProcessData : IProcessData
    {
        private readonly ILogger<ProcessData> _logger;
        public ProcessData(ILogger<ProcessData> logger)
        {
            _logger = logger;
        }
        public void Process(string message)
        {
            _logger.LogDebug($"You get message: {message}");
            // TODO: another handler
        }
    }
}
