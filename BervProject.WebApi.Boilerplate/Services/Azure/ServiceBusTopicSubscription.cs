using Amazon.Runtime.Internal;
using Azure.Messaging.ServiceBus;
using BervProject.WebApi.Boilerplate.ConfigModel;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
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
        private readonly ServiceBusProcessor _serviceBusProcessor;
        private readonly IProcessData _processData;
        public ServiceBusTopicSubscription(ILogger<ServiceBusTopicSubscription> logger,
            IProcessData processData,
            AzureConfiguration azureConfiguration,
            ServiceBusClient serviceBusClient)
        {
            _logger = logger;
            _processData = processData;
            _topicName = azureConfiguration.ServiceBus.TopicName;
            var options = new ServiceBusProcessorOptions
            {
                // By default or when AutoCompleteMessages is set to true, the processor will complete the message after executing the message handler
                // Set AutoCompleteMessages to false to [settle messages](https://docs.microsoft.com/en-us/azure/service-bus-messaging/message-transfers-locks-settlement#peeklock) on your own.
                // In both cases, if the message handler throws an exception without settling the message, the processor will abandon the message.
                AutoCompleteMessages = false,

                // I can also allow for multi-threading
                MaxConcurrentCalls = 2
            };
            _serviceBusProcessor = serviceBusClient.CreateProcessor(_topicName, options);
        }

        public async Task CloseSubscriptionClientAsync()
        {
            await _serviceBusProcessor.CloseAsync();
        }

        public void RegisterOnMessageHandlerAndReceiveMessages()
        {

            _logger.LogDebug($"Register topic for {_topicName}/{_topicSubscription}");
            _serviceBusProcessor.ProcessMessageAsync += MessageHandler;
            _serviceBusProcessor.ProcessErrorAsync += ErrorHandler;
            _logger.LogDebug($"Registered topic for {_topicName}/{_topicSubscription}");
        }

        private async Task MessageHandler(ProcessMessageEventArgs args)
        {
            var myPayload = args.Message.Body.ToString();
            _processData.Process(myPayload);
            await args.CompleteMessageAsync(args.Message);
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            _logger.LogError(args.Exception, "Message handler encountered an exception");

            _logger.LogDebug($"- Error Source: {args.ErrorSource}");
            _logger.LogDebug($"- Entity Path: {args.EntityPath}");
            _logger.LogDebug($"- Identifier: {args.Identifier}");
            _logger.LogDebug($"- FullyQualifiedNamespace: {args.FullyQualifiedNamespace}");

            return Task.CompletedTask;
        }
    }
}
