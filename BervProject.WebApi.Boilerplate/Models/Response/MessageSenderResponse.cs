namespace BervProject.WebApi.Boilerplate.Models.Response;

/// <summary>
/// Message Sender Response
/// </summary>
public class MessageSenderResponse
{
    /// <summary>
    /// Is Success
    /// </summary>
    public bool IsSuccess { get; set; }
    /// <summary>
    /// Your Message
    /// </summary>
    public string YourMessage { get; set; }
}