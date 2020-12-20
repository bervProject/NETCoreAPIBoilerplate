using BervProject.WebApi.Boilerplate.ConfigModel;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BervProject.WebApi.Boilerplate.Services
{
    public class ServiceBusConsumer : IServiceBusConsumer
    {
        private readonly ILogger<ServiceBusConsumer> _logger;
        private readonly string _queueName;
        private readonly string _connectionString;
        private readonly QueueClient _queueClient;
        private readonly IProcessData _processData;
        public ServiceBusConsumer(ILogger<ServiceBusConsumer> logger,
            IProcessData processData,
            AzureConfiguration azureConfiguration)
        {
            _logger = logger;
            _processData = processData;
            _connectionString = azureConfiguration.ServiceBus.ConnectionString;
            _queueName = azureConfiguration.ServiceBus.QueueName;
            _queueClient = new QueueClient(_connectionString, _queueName);
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

            _queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
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
