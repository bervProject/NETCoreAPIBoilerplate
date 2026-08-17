namespace BervProject.WebApi.Boilerplate.Services.Azure;

using System.Threading.Tasks;

/// <summary>
/// Service Bus Topic Sub
/// </summary>
public interface IServiceBusTopicSubscription
{
    /// <summary>
    /// Receive topic registration
    /// </summary>
    void RegisterOnMessageHandlerAndReceiveMessages();
    /// <summary>
    /// Close topic subs
    /// </summary>
    /// <returns></returns>
    Task CloseSubscriptionClientAsync();
}