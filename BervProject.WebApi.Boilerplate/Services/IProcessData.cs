namespace BervProject.WebApi.Boilerplate.Services;

/// <summary>
/// Interface Process Message
/// </summary>
public interface IProcessData
{
    /// <summary>
    /// Process message
    /// </summary>
    /// <param name="message"></param>
    void Process(string message);
}