using Azure.Messaging.ServiceBus;

namespace BervProject.WebApi.Boilerplate.Services.Azure;

using ConfigModel;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

/// <inheritdoc />
public class AzureQueueServices : IAzureQueueServices
{
    private readonly string _queueName;
    private readonly ServiceBusSender _serviceBusSender;
    private readonly ILogger<AzureQueueServices> _logger;
    /// <summary>
    /// Default constructor with dependency injections
    /// </summary>
    /// <param name="azureConfiguration"></param>
    /// <param name="logger"></param>
    /// <param name="serviceBusClient"></param>
    public AzureQueueServices(AzureConfiguration azureConfiguration, ILogger<AzureQueueServices> logger, ServiceBusClient serviceBusClient)
    {
        _logger = logger;
        _queueName = azureConfiguration.ServiceBus.QueueName;
        _serviceBusSender = serviceBusClient.CreateSender(_queueName);
    }

    /// <inheritdoc />
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