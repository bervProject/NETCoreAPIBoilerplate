namespace BervProject.WebApi.Test;

using Autofac;
using Autofac.Extras.Moq;
using Boilerplate.Controllers;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using System;
using System.Linq;
using System.Text;
using Xunit;

public class WeatherForecastTest
{
    private static WeatherForecastController CreateController(AutoMock mock)
    {
        Environment.SetEnvironmentVariable(
            "APPLICATIONINSIGHTS_CONNECTION_STRING",
            "InstrumentationKey=00000000-0000-0000-0000-000000000000");
        var telemetryConfiguration = TelemetryConfiguration.CreateDefault();
        var telemetryClient = new TelemetryClient(telemetryConfiguration);
        return mock.Create<WeatherForecastController>(new TypedParameter(typeof(TelemetryClient), telemetryClient));
    }

    [Fact]
    public void GetTest()
    {
        using var mock = AutoMock.GetLoose();
        var controller = CreateController(mock);
        var result = controller.Get();
        Assert.Equal(5, result.Count());
    }

    [Fact]
    public void GetCacheTest()
    {
        using var mock = AutoMock.GetLoose();
        mock.Mock<IDistributedCache>().Setup(x => x.Get(It.IsAny<string>())).Returns((byte[])null);
        var cacheMock = mock.Create<IDistributedCache>();
        var controller = CreateController(mock);
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
        var controller = CreateController(mock);
        var result = controller.GetCache(cacheMock);
        Assert.Equal(expectedResult, result.Value);
    }

    [Fact]
    public void TriggerExceptionTest()
    {
        using var mock = AutoMock.GetLoose();
        var controller = CreateController(mock);
        var result = Assert.Throws<Exception>(() => controller.TriggerException());
        Assert.Equal("Unhandled Exception", result.Message);
    }
}
