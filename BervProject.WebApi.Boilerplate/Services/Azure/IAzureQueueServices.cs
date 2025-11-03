namespace BervProject.WebApi.Boilerplate.Services.Azure;

using System.Threading.Tasks;

/// <summary>
/// Azure Queue Service
/// </summary>
public interface IAzureQueueServices
{
    /// <summary>
    /// Send Message
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    Task<bool> SendMessage(string message);
}