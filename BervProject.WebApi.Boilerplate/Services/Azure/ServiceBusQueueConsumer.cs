using BervProject.WebApi.Boilerplate.ConfigModel;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BervProject.WebApi.Boilerplate.Services.Azure
{
    public class ServiceBusQueueConsumer : IServiceBusQueueConsumer
    {
        private readonly ILogger<ServiceBusQueueConsumer> _logger;
        private readonly string _queueName;
        private readonly QueueClient _queueClient;
        private readonly IProcessData _processData;
        public ServiceBusQueueConsumer(ILogger<ServiceBusQueueConsumer> logger,
            IProcessData processData,
            AzureConfiguration azureConfiguration)
        {
            _logger = logger;
            _processData = processData;
            _queueName = azureConfiguration.ServiceBus.QueueName;
            var connectionString = azureConfiguration.ServiceBus.ConnectionString;
            _queueClient = new QueueClient(connectionString, _queueName);
        }

        public async Task CloseQueueAsync()
        {
            await _queueClient.CloseAsync();
        }

        public void RegisterOnMessageHandlerAndReceiveMessages()
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };

            _logger.LogDebug($"Register queue for {_queueName}");
            _queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
            _logger.LogDebug($"Registered queue for {_queueName}");
        }

        private async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            var myPayload = Encoding.UTF8.GetString(message.Body);
            _processData.Process(myPayload);
            await _queueClient.CompleteAsync(message.SystemProperties.LockToken);
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
