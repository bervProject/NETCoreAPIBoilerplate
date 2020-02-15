using Amazon;
using Amazon.Runtime;
using Amazon.Runtime.Internal.Util;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using BervProject.WebApi.Boilerplate.ConfigModel;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace BervProject.WebApi.Boilerplate.Services
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

        public void SendEmail(List<string> receiver)
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
            var task = _emailClient.SendEmailAsync(request);
            task.Wait();
            _logger.LogWarning($"Message id: {task.Result.MessageId}");
            if (task.Result.HttpStatusCode == System.Net.HttpStatusCode.OK)
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
