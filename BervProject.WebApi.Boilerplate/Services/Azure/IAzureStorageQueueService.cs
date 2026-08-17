namespace BervProject.WebApi.Boilerplate.Services.Azure;

/// <summary>
/// Azure Storage Queue Service
/// </summary>
public interface IAzureStorageQueueService
{
    /// <summary>
    /// Send Message
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    bool SendMessage(string message);
    /// <summary>
    /// Receive Message
    /// </summary>
    /// <returns></returns>
    string ReceiveMessage();
}