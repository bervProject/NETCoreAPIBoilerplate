using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Autofac.Extras.Moq;
using BervProject.WebApi.Boilerplate.Services.AWS;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace BervProject.WebApi.Test.Services
{
    public class EmailServiceTest
    {
        [Fact]
        public async Task SendEmailSuccess()
        {
            using var mock = AutoMock.GetLoose();
            var mockEmailService = mock.Mock<IAmazonSimpleEmailService>();
            var logMock = mock.Mock<ILogger<EmailService>>();
            var reciever = new List<String>
            {
                "myreceiver@receiver.com"
            };
            var sendEmailResponse = new SendEmailResponse()
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                MessageId = "random",
                ResponseMetadata = new Amazon.Runtime.ResponseMetadata()
            };
            mockEmailService.Setup(x => x.SendEmailAsync(It.IsAny<SendEmailRequest>(), default))
                .Returns(Task.FromResult(sendEmailResponse));
            var emailService = mock.Create<EmailService>();
            await emailService.SendEmail(reciever);
            mockEmailService.Verify(x => x.SendEmailAsync(It.IsAny<SendEmailRequest>(), default), Times.Once());
        }

        [Fact]
        public async Task SendEmailFailed()
        {
            using var mock = AutoMock.GetLoose();
            var mockEmailService = mock.Mock<IAmazonSimpleEmailService>();
            var logMock = mock.Mock<ILogger<EmailService>>();
            var reciever = new List<String>
            {
                "myreceiver@receiver.com"
            };
            var sendEmailResponse = new SendEmailResponse()
            {
                HttpStatusCode = System.Net.HttpStatusCode.InternalServerError,
                MessageId = "random",
                ResponseMetadata = new Amazon.Runtime.ResponseMetadata()
            };
            mockEmailService.Setup(x => x.SendEmailAsync(It.IsAny<SendEmailRequest>(), default))
                .Returns(Task.FromResult(sendEmailResponse));
            var emailService = mock.Create<EmailService>();
            await emailService.SendEmail(reciever);
            mockEmailService.Verify(x => x.SendEmailAsync(It.IsAny<SendEmailRequest>(), default), Times.Once());
        }
    }
}
