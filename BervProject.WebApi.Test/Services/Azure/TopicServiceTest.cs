using Azure.Messaging.ServiceBus;

namespace BervProject.WebApi.Test.Services.Azure;

using Autofac;
using Autofac.Extras.Moq;
using Boilerplate.ConfigModel;
using BervProject.WebApi.Boilerplate.Services.Azure;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;

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