using Azure.Messaging.ServiceBus;

namespace BervProject.WebApi.Boilerplate.Services.Azure;

using ConfigModel;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

/// <inheritdoc />
public class TopicServices : ITopicServices
{
    private readonly string _topicName;
    private readonly ServiceBusSender _serviceBusSender;
    private readonly ILogger<TopicServices> _logger;
    /// <summary>
    /// Default constructor with dependency injections
    /// </summary>
    /// <param name="azureConfiguration"></param>
    /// <param name="logger"></param>
    /// <param name="serviceBusClient"></param>
    public TopicServices(AzureConfiguration azureConfiguration, ILogger<TopicServices> logger, ServiceBusClient serviceBusClient)
    {
        _logger = logger;
        _topicName = azureConfiguration.ServiceBus.TopicName;
        _serviceBusSender = serviceBusClient.CreateSender(_topicName);
    }

    /// <inheritdoc />
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