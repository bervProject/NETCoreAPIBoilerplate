using Autofac;
using Autofac.Extras.Moq;
using Azure.Messaging.ServiceBus;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using BervProject.WebApi.Boilerplate.ConfigModel;
using BervProject.WebApi.Boilerplate.Services.Azure;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BervProject.WebApi.Test.Services.Azure
{
    public class TopicServiceTest
    {
        [Fact]
        public async Task SendHelloToTopicServiceBus()
        {
            var azureConfig = new AzureConfiguration
            {
                ServiceBus = new AzureServiceBus
                {
                    TopicName = ""
                }
            };
            var message = "Hello!";
            using var mock = AutoMock.GetLoose(cfg => cfg.RegisterInstance(azureConfig).As<AzureConfiguration>());
            var loggerMock = mock.Mock<ILogger<TopicServices>>();
            var serviceBusClient = mock.Mock<ServiceBusClient>();
            var serviceBusSender = mock.Mock<ServiceBusSender>();
            serviceBusClient.Setup(x => x.CreateSender("")).Returns(serviceBusSender.Object);
            serviceBusSender.Setup(x => x.SendMessageAsync(It.Is<ServiceBusMessage>(x => x.Body.ToString() == message), default)).Returns(Task.CompletedTask);
            var queueService = mock.Create<TopicServices>();
            var ok = await queueService.SendTopic("Hello!");
            serviceBusSender.Verify(x => x.SendMessageAsync(It.Is<ServiceBusMessage>(x => x.Body.ToString() == message), default), Times.Once());
            serviceBusSender.Verify(x => x.CloseAsync(default), Times.Once);
            Assert.True(ok);
        }

        [Fact]
        public async Task SendMessageFailedTest()
        {
            var azureConfig = new AzureConfiguration
            {
                ServiceBus = new AzureServiceBus
                {
                    TopicName = ""
                }
            };
            var message = "Hello!";
            using var mock = AutoMock.GetLoose(cfg => cfg.RegisterInstance(azureConfig).As<AzureConfiguration>());
            var loggerMock = mock.Mock<ILogger<TopicServices>>();
            var serviceBusClient = mock.Mock<ServiceBusClient>();
            var serviceBusSender = mock.Mock<ServiceBusSender>();
            serviceBusClient.Setup(x => x.CreateSender("")).Returns(serviceBusSender.Object);
            serviceBusSender.Setup(x => x.SendMessageAsync(It.Is<ServiceBusMessage>(x => x.Body.ToString() == message), default)).Throws<ServiceBusException>();
            var queueService = mock.Create<TopicServices>();
            var ok = await queueService.SendTopic("Hello!");
            serviceBusSender.Verify(x => x.SendMessageAsync(It.Is<ServiceBusMessage>(x => x.Body.ToString() == message), default), Times.Once());
            serviceBusSender.Verify(x => x.CloseAsync(default), Times.Once);
            Assert.False(ok);
        }
    }
}
