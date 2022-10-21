using Autofac.Extras.Moq;
using BervProject.WebApi.Boilerplate.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace BervProject.WebApi.Test.Services
{
    public class ProcessDataTest
    {

        [Fact]
        public void ProcessDataSuccess()
        {
            using var mock = AutoMock.GetLoose();
            var loggerMock = mock.Mock<ILogger<ProcessData>>();
            var processData = mock.Create<ProcessData>();
            processData.Process("Hello!");
            loggerMock.Verify(logger => logger.Log(
                    It.Is<LogLevel>(logLevel => logLevel == LogLevel.Debug),
                    It.Is<EventId>(eventId => eventId.Id == 0),
                    It.Is<It.IsAnyType>((@object, @type) => @object.ToString() == "You get message: Hello!" && @type.Name == "FormattedLogValues"),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}
