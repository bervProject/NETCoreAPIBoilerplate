using Azure.Messaging.ServiceBus;
using BervProject.WebApi.Boilerplate.ConfigModel;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BervProject.WebApi.Boilerplate.Services.Azure
{
    public class AzureQueueServices : IAzureQueueServices
    {
        private readonly string _queueName;
        private readonly ServiceBusSender _serviceBusSender;
        private readonly ILogger<AzureQueueServices> _logger;
        public AzureQueueServices(AzureConfiguration azureConfiguration, ILogger<AzureQueueServices> logger, ServiceBusClient serviceBusClient)
        {
            _logger = logger;
            _queueName = azureConfiguration.ServiceBus.QueueName;
            _serviceBusSender = serviceBusClient.CreateSender(_queueName);
        }

        public async Task<bool> SendMessage(string message)
        {
            try
            {
                var messageQueue = new ServiceBusMessage(message);
                _logger.LogDebug($"Sending message: {message}");
                await _serviceBusSender.SendMessageAsync(messageQueue);
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
                await _serviceBusSender.CloseAsync();
            }
        }
    }
}
