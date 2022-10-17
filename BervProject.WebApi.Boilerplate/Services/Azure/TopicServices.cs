using Azure.Messaging.ServiceBus;
using BervProject.WebApi.Boilerplate.ConfigModel;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Threading.Tasks;

namespace BervProject.WebApi.Boilerplate.Services.Azure
{
    public class TopicServices : ITopicServices
    {
        private readonly string _topicName;
        private readonly ServiceBusSender _serviceBusSender;
        private readonly ILogger<TopicServices> _logger;
        public TopicServices(AzureConfiguration azureConfiguration, ILogger<TopicServices> logger, ServiceBusClient serviceBusClient)
        {
            _logger = logger;
            _topicName = azureConfiguration.ServiceBus.TopicName;
            _serviceBusSender = serviceBusClient.CreateSender(_topicName);
        }
        public async Task<bool> SendTopic(string message)
        {
            try
            {
                var encodedMessage = new ServiceBusMessage(message);
                _logger.LogDebug($"Sending message: {message}");
                await _serviceBusSender.SendMessageAsync(encodedMessage);
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
