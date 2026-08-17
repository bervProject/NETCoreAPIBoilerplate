namespace BervProject.WebApi.Boilerplate.Services.AWS;

using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

/// <inheritdoc />
public class EmailService : IEmailService
{
    private readonly IAmazonSimpleEmailService _emailClient;
    private readonly ILogger<EmailService> _logger;
    /// <summary>
    /// Default Constructor with Dependency Injections
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="emailClient"></param>
    public EmailService(ILogger<EmailService> logger, IAmazonSimpleEmailService emailClient)
    {
        _logger = logger;
        _emailClient = emailClient;
    }

    /// <summary>
    /// Send Email
    /// </summary>
    /// <param name="receiver"></param>
    public async Task SendEmail(List<string> receiver)
    {
        var request = new SendEmailRequest()
        {
            ReplyToAddresses = new List<string> { "bervianto.leo@gmail.com" },
            Message = new Message()
            {
                Body = new Body(new Content("Hello World!")),
                Subject = new Content("Stand by me")
            },
            Destination = new Destination(receiver),
            Source = "support@berviantoleo.my.id"
        };
        var response = await _emailClient.SendEmailAsync(request);
        string messageId = $"Message id: {response.MessageId}";
        _logger.LogDebug(messageId);
        if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
        {
            _logger.LogInformation("Finished Sent Email");
        }
        else
        {
            _logger.LogWarning("There is a problem when sending email");
            string message = $"Error: {response.MessageId}:{response.HttpStatusCode}:{JsonSerializer.Serialize(response.ResponseMetadata.Metadata)}";
            _logger.LogWarning(message);
        }
    }
}