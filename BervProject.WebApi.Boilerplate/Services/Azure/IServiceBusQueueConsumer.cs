namespace BervProject.WebApi.Boilerplate.Services.Azure;

using System.Threading.Tasks;

/// <summary>
/// Service Bus Queue Consumer
/// </summary>
public interface IServiceBusQueueConsumer
{
    /// <summary>
    /// Register event
    /// </summary>
    void RegisterOnMessageHandlerAndReceiveMessages();
    /// <summary>
    /// Close Queue
    /// </summary>
    /// <returns></returns>
    Task CloseQueueAsync();
}