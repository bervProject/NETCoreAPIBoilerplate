namespace BervProject.WebApi.Boilerplate.Services.Azure;

using System.Threading.Tasks;

/// <summary>
/// Topic Service
/// </summary>
public interface ITopicServices
{
    /// <summary>
    /// Sending topic
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    Task<bool> SendTopic(string message);
}