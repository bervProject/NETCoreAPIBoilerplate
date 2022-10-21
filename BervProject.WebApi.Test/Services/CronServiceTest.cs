using Autofac.Extras.Moq;
using BervProject.WebApi.Boilerplate.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace BervProject.WebApi.Test.Services
{
    public class CronServiceTest
    {

        [Fact]
        public void ProcessDataSuccess()
        {
            using var mock = AutoMock.GetLoose();
            var loggerMock = mock.Mock<ILogger<CronService>>();
            var loggerFactoryMock = mock.Mock<ILoggerFactory>();
            loggerFactoryMock.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(loggerMock.Object);
            var processData = mock.Create<CronService>();
            processData.HelloWorld();
            loggerMock.Verify(logger => logger.Log(
                    It.Is<LogLevel>(logLevel => logLevel == LogLevel.Debug),
                    It.Is<EventId>(eventId => eventId.Id == 0),
                    It.Is<It.IsAnyType>((@object, @type) => @object.ToString() == "Run Cron" && @type.Name == "FormattedLogValues"),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
            loggerMock.Verify(logger => logger.Log(
                    It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
                    It.Is<EventId>(eventId => eventId.Id == 0),
                    It.Is<It.IsAnyType>((@object, @type) => @object.ToString() == "Hello World Cron!" && @type.Name == "FormattedLogValues"),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
            loggerMock.Verify(logger => logger.Log(
                    It.Is<LogLevel>(logLevel => logLevel == LogLevel.Debug),
                    It.Is<EventId>(eventId => eventId.Id == 0),
                    It.Is<It.IsAnyType>((@object, @type) => @object.ToString() == "Finished Run Cron" && @type.Name == "FormattedLogValues"),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}
