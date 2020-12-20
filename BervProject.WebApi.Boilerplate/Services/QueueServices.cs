using BervProject.WebApi.Boilerplate.ConfigModel;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Threading.Tasks;

namespace BervProject.WebApi.Boilerplate.Services
{
    public class QueueServices : IQueueServices
    {
        private readonly string _serviceBusConnectionString;
        private readonly string _queueName;
        private readonly QueueClient _queueClient;
        private readonly ILogger<QueueServices> _logger;
        public QueueServices(AzureConfiguration azureConfiguration, ILogger<QueueServices> logger)
        {
            _logger = logger;
            _serviceBusConnectionString = azureConfiguration.ServiceBus.ConnectionString;
            _queueName = azureConfiguration.ServiceBus.QueueName;
            _queueClient = new QueueClient(_serviceBusConnectionString, _queueName);
        }

        public async Task<bool> SendMessage(string message)
        {
            try
            {
                var messageQueue = new Message(Encoding.UTF8.GetBytes(message));
                _logger.LogDebug($"Sending message: {message}");
                await _queueClient.SendAsync(messageQueue);
                _logger.LogDebug($"Sent message: {message}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return false;
            }
            finally
            {
                await _queueClient.CloseAsync();
            }
        }
    }
}
