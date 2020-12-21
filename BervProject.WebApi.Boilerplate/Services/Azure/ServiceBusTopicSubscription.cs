using BervProject.WebApi.Boilerplate.ConfigModel;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BervProject.WebApi.Boilerplate.Services.Azure
{
    public class ServiceBusTopicSubscription : IServiceBusTopicSubscription
    {
        private readonly ILogger<ServiceBusTopicSubscription> _logger;
        private readonly string _topicName;
        private readonly string _topicSubscription = "topicSubscriptionRandom";
        private readonly SubscriptionClient _subscriptionClient;
        private readonly IProcessData _processData;
        public ServiceBusTopicSubscription(ILogger<ServiceBusTopicSubscription> logger,
            IProcessData processData,
            AzureConfiguration azureConfiguration)
        {
            _logger = logger;
            _processData = processData;
            _topicName = azureConfiguration.ServiceBus.TopicName;
            var connectionString = azureConfiguration.ServiceBus.ConnectionString;
            _subscriptionClient = new SubscriptionClient(connectionString, _topicName, _topicSubscription);
        }

        public async Task CloseSubscriptionClientAsync()
        {
            await _subscriptionClient.CloseAsync();
        }

        public void RegisterOnMessageHandlerAndReceiveMessages()
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };

            _logger.LogDebug($"Register topic for {_topicName}/{_topicSubscription}");
            _subscriptionClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
            _logger.LogDebug($"Registered topic for {_topicName}/{_topicSubscription}");
        }

        private async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            var myPayload = Encoding.UTF8.GetString(message.Body);
            _processData.Process(myPayload);
            await _subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            _logger.LogError(exceptionReceivedEventArgs.Exception, "Message handler encountered an exception");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;

            _logger.LogDebug($"- Endpoint: {context.Endpoint}");
            _logger.LogDebug($"- Entity Path: {context.EntityPath}");
            _logger.LogDebug($"- Executing Action: {context.Action}");

            return Task.CompletedTask;
        }
    }
}
