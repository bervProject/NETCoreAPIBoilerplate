using Azure.Storage.Queues;

namespace BervProject.WebApi.Boilerplate.Services.Azure;

using ConfigModel;
using Microsoft.Extensions.Logging;
using System;

/// <inheritdoc />
public class AzureStorageQueueService : IAzureStorageQueueService
{
    private readonly ILogger<AzureStorageQueueService> _logger;
    private readonly QueueClient _queueClient;
    private readonly string _queueName;

    /// <summary>
    /// Default constructor with dependency injections
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="azureConfiguration"></param>
    /// <param name="queueServiceClient"></param>
    public AzureStorageQueueService(ILogger<AzureStorageQueueService> logger,
        AzureConfiguration azureConfiguration,
        QueueServiceClient queueServiceClient)
    {
        _logger = logger;
        _queueName = azureConfiguration.Storage.Queue.QueueName;
        _queueClient = queueServiceClient.GetQueueClient(_queueName);
        _queueClient.CreateIfNotExists();
    }

    /// <inheritdoc />
    public string ReceiveMessage()
    {
        try
        {
            if (_queueClient.Exists())
            {
                var response = _queueClient.ReceiveMessage();
                var message = response?.Value;
                if (message != null)
                {
                    var textMessage = message.Body.ToString();
                    _logger.LogDebug($"Get message from {_queueName}:{message.MessageId}: {textMessage}");
                    _logger.LogDebug($"Message {message.MessageId} deqeue from {_queueName}");
                    var responseDelete = _queueClient.DeleteMessage(message.MessageId, message.PopReceipt);
                    _logger.LogDebug($"Message finished deqeue from {_queueName}: {responseDelete.ClientRequestId}");
                    return textMessage;
                }

                _logger.LogDebug($"Empty message at {_queueName}");
                return null;
            }

            _logger.LogWarning($"{_queueName} is not exists");
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Something Error, ignoring");
            return null;
        }
    }

    /// <inheritdoc />
    public bool SendMessage(string message)
    {
        try
        {
            if (_queueClient.Exists())
            {
                _logger.LogDebug($"Sending message: {message} at {_queueName}");
                var response = _queueClient.SendMessage(message);
                var messageId = response?.Value?.MessageId;
                _logger.LogDebug($"Sent message to {_queueName} with id: {messageId}");
                return true;
            }

            _logger.LogWarning($"{_queueName} is not exists");
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return false;
        }
    }
}