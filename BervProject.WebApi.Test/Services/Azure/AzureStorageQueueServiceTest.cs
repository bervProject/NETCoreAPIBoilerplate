using Autofac;
using Autofac.Extras.Moq;
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
    public class AzureStorageQueueServiceTest
    {
        [Fact]
        public void ReceiveEmptyMessageTest()
        {
            var azureConfig = new AzureConfiguration
            {
                Storage = new AzureStorage
                {
                    Queue = new StorageQueue
                    {
                        QueueName = ""
                    }
                }
            };
            using var mock = AutoMock.GetLoose(cfg => cfg.RegisterInstance(azureConfig).As<AzureConfiguration>());
            var loggerMock = mock.Mock<ILogger<AzureStorageQueueService>>();
            var queueServiceClient = mock.Mock<QueueServiceClient>();
            var mockQueue = mock.Mock<QueueClient>();
            queueServiceClient.Setup(x => x.GetQueueClient("")).Returns(mockQueue.Object);
            mockQueue.Setup(x => x.CreateIfNotExists(null, default));
            mockQueue.Setup(x => x.Exists(default).Value).Returns(true);
            mockQueue.Setup(x => x.ReceiveMessage(It.IsAny<TimeSpan?>(), default).Value).Returns(() => null);
            var queueService = mock.Create<AzureStorageQueueService>();
            var emptyResult = queueService.ReceiveMessage();
            mockQueue.Verify(x => x.Exists(default), Times.Once());
            mockQueue.Verify(x => x.ReceiveMessage(It.IsAny<TimeSpan?>(), default), Times.Once());
            Assert.Null(emptyResult);
        }

        [Fact]
        public void ReceiveEmptyMessageBecauseQueueNotFoundTest()
        {
            var azureConfig = new AzureConfiguration
            {
                Storage = new AzureStorage
                {
                    Queue = new StorageQueue
                    {
                        QueueName = ""
                    }
                }
            };
            using var mock = AutoMock.GetLoose(cfg => cfg.RegisterInstance(azureConfig).As<AzureConfiguration>());
            var loggerMock = mock.Mock<ILogger<AzureStorageQueueService>>();
            var queueServiceClient = mock.Mock<QueueServiceClient>();
            var mockQueue = mock.Mock<QueueClient>();
            queueServiceClient.Setup(x => x.GetQueueClient("")).Returns(mockQueue.Object);
            mockQueue.Setup(x => x.CreateIfNotExists(null, default));
            mockQueue.Setup(x => x.Exists(default).Value).Returns(false);
            mockQueue.Setup(x => x.ReceiveMessage(It.IsAny<TimeSpan?>(), default).Value).Returns(() => null);
            var queueService = mock.Create<AzureStorageQueueService>();
            var emptyResult = queueService.ReceiveMessage();
            mockQueue.Verify(x => x.Exists(default), Times.Once());
            mockQueue.Verify(x => x.ReceiveMessage(It.IsAny<TimeSpan?>(), default), Times.Never());
            Assert.Null(emptyResult);
        }

        [Fact]
        public void ReceiveEmptyMessageBecauseExceptionTest()
        {
            var azureConfig = new AzureConfiguration
            {
                Storage = new AzureStorage
                {
                    Queue = new StorageQueue
                    {
                        QueueName = ""
                    }
                }
            };
            using var mock = AutoMock.GetLoose(cfg => cfg.RegisterInstance(azureConfig).As<AzureConfiguration>());
            var loggerMock = mock.Mock<ILogger<AzureStorageQueueService>>();
            var queueServiceClient = mock.Mock<QueueServiceClient>();
            var mockQueue = mock.Mock<QueueClient>();
            queueServiceClient.Setup(x => x.GetQueueClient("")).Returns(mockQueue.Object);
            mockQueue.Setup(x => x.CreateIfNotExists(null, default));
            mockQueue.Setup(x => x.Exists(default).Value).Returns(false);
            mockQueue.Setup(x => x.ReceiveMessage(It.IsAny<TimeSpan?>(), default).Value).Throws<Exception>();
            var queueService = mock.Create<AzureStorageQueueService>();
            var emptyResult = queueService.ReceiveMessage();
            mockQueue.Verify(x => x.Exists(default), Times.Once());
            mockQueue.Verify(x => x.ReceiveMessage(It.IsAny<TimeSpan?>(), default), Times.Never());
            Assert.Null(emptyResult);
        }

        [Fact]
        public void SendMessageSuccessTest()
        {
            var azureConfig = new AzureConfiguration
            {
                Storage = new AzureStorage
                {
                    Queue = new StorageQueue
                    {
                        QueueName = ""
                    }
                }
            };
            using var mock = AutoMock.GetLoose(cfg => cfg.RegisterInstance(azureConfig).As<AzureConfiguration>());
            var loggerMock = mock.Mock<ILogger<AzureStorageQueueService>>();
            var queueServiceClient = mock.Mock<QueueServiceClient>();
            var mockQueue = mock.Mock<QueueClient>();
            queueServiceClient.Setup(x => x.GetQueueClient("")).Returns(mockQueue.Object);
            mockQueue.Setup(x => x.CreateIfNotExists(null, default));
            mockQueue.Setup(x => x.Exists(default).Value).Returns(true);
            var message = "boba";
            mockQueue.Setup(x => x.SendMessage(It.IsAny<string>(), default)).Returns(() => null);
            var queueService = mock.Create<AzureStorageQueueService>();
            var success = queueService.SendMessage(message);
            mockQueue.Verify(x => x.Exists(default), Times.Once());
            Assert.True(success);
        }

        [Fact]
        public void SendMessageFailedTest()
        {
            var azureConfig = new AzureConfiguration
            {
                Storage = new AzureStorage
                {
                    Queue = new StorageQueue
                    {
                        QueueName = ""
                    }
                }
            };
            using var mock = AutoMock.GetLoose(cfg => cfg.RegisterInstance(azureConfig).As<AzureConfiguration>());
            var loggerMock = mock.Mock<ILogger<AzureStorageQueueService>>();
            var queueServiceClient = mock.Mock<QueueServiceClient>();
            var mockQueue = mock.Mock<QueueClient>();
            queueServiceClient.Setup(x => x.GetQueueClient("")).Returns(mockQueue.Object);
            mockQueue.Setup(x => x.CreateIfNotExists(null, default));
            mockQueue.Setup(x => x.Exists(default).Value).Returns(false);
            var message = "boba";
            mockQueue.Setup(x => x.SendMessage(It.IsAny<string>(), default)).Returns(() => null);
            var queueService = mock.Create<AzureStorageQueueService>();
            var success = queueService.SendMessage(message);
            mockQueue.Verify(x => x.Exists(default), Times.Once());
            Assert.False(success);
        }

        [Fact]
        public void SendMessageFailedBecauseExceptionTest()
        {
            var azureConfig = new AzureConfiguration
            {
                Storage = new AzureStorage
                {
                    Queue = new StorageQueue
                    {
                        QueueName = ""
                    }
                }
            };
            using var mock = AutoMock.GetLoose(cfg => cfg.RegisterInstance(azureConfig).As<AzureConfiguration>());
            var loggerMock = mock.Mock<ILogger<AzureStorageQueueService>>();
            var queueServiceClient = mock.Mock<QueueServiceClient>();
            var mockQueue = mock.Mock<QueueClient>();
            queueServiceClient.Setup(x => x.GetQueueClient("")).Returns(mockQueue.Object);
            mockQueue.Setup(x => x.CreateIfNotExists(null, default));
            mockQueue.Setup(x => x.Exists(default).Value).Returns(false);
            var message = "boba";
            mockQueue.Setup(x => x.SendMessage(It.IsAny<string>(), default)).Throws<Exception>();
            var queueService = mock.Create<AzureStorageQueueService>();
            var success = queueService.SendMessage(message);
            mockQueue.Verify(x => x.Exists(default), Times.Once());
            Assert.False(success);
        }
    }
}
