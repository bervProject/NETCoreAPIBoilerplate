using Autofac.Extras.Moq;
using BervProject.WebApi.Boilerplate.Controllers;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using System;
using System.Linq;
using System.Text;
using Xunit;

namespace BervProject.WebApi.Test
{
    public class WeatherForecastTest
    {
        [Fact]
        public void GetTest()
        {
            using var mock = AutoMock.GetLoose();
            var controller = mock.Create<WeatherForecastController>();
            var result = controller.Get();
            Assert.Equal(5, result.Count());
        }

        [Fact]
        public void GetCacheTest()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IDistributedCache>().Setup(x => x.Get(It.IsAny<string>())).Returns((byte[])null);
            var cacheMock = mock.Create<IDistributedCache>();
            var controller = mock.Create<WeatherForecastController>();
            var result = controller.GetCache(cacheMock);
            Assert.Equal("", result.Value);
        }

        [Fact]
        public void GetCacheResultTest()
        {
            using var mock = AutoMock.GetLoose();
            const string expectedResult = "I know anything";
            var expectedByte = Encoding.ASCII.GetBytes(expectedResult);
            mock.Mock<IDistributedCache>().Setup(x => x.Get(It.IsAny<string>())).Returns(expectedByte);
            var cacheMock = mock.Create<IDistributedCache>();
            var controller = mock.Create<WeatherForecastController>();
            var result = controller.GetCache(cacheMock);
            Assert.Equal(expectedResult, result.Value);
        }

        [Fact]
        public void TriggerExceptionTest()
        {
            using var mock = AutoMock.GetLoose();
            var controller = mock.Create<WeatherForecastController>();
            var result = Assert.Throws<Exception>(() => controller.TriggerException());
            Assert.Equal("Unhandled Exception", result.Message);
        }
    }
}
