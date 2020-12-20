using BervProject.WebApi.Boilerplate.ConfigModel;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Threading.Tasks;

namespace BervProject.WebApi.Boilerplate.Services
{
    public class TopicServices : ITopicServices
    {
        private readonly string _serviceBusConnectionString;
        private readonly string _topicName;
        private readonly TopicClient _topicClient;
        private readonly ILogger<TopicServices> _logger;
        public TopicServices(AzureConfiguration azureConfiguration, ILogger<TopicServices> logger)
        {
            _logger = logger;
            _serviceBusConnectionString = azureConfiguration.ServiceBus.ConnectionString;
            _topicName = azureConfiguration.ServiceBus.TopicName;
            _topicClient = new TopicClient(_serviceBusConnectionString, _topicName);
        }
        public async Task<bool> SendTopic(string message)
        {
            try
            {
                var encodedMessage = new Message(Encoding.UTF8.GetBytes(message));
                _logger.LogDebug($"Sending message: {message}");
                await _topicClient.SendAsync(encodedMessage);
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
                await _topicClient.CloseAsync();
            }
        }
    }
}
