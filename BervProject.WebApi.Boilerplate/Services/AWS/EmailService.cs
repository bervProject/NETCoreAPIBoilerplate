using Amazon;
using Amazon.Runtime;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using BervProject.WebApi.Boilerplate.ConfigModel;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BervProject.WebApi.Boilerplate.Services.AWS
{
    public class EmailService : IEmailService
    {
        private readonly AmazonSimpleEmailServiceClient _emailClient;
        private readonly ILogger<EmailService> _logger;
        public EmailService(ILogger<EmailService> logger, AWSConfiguration awsConfig)
        {
            _logger = logger;
            var credentials = new BasicAWSCredentials(awsConfig.Basic.Auth.AccessKey, awsConfig.Basic.Auth.SecretKey);
            _emailClient = new AmazonSimpleEmailServiceClient(credentials, RegionEndpoint.GetBySystemName(awsConfig.Email.Location));
        }

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
            _logger.LogWarning($"Message id: {response.MessageId}");
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                _logger.LogInformation("Finished Sent Email");
            }
            else
            {
                _logger.LogWarning("There is a problem when sending email");
            }
        }
    }
}
