using BervProject.WebApi.Boilerplate.Controllers;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using System.Linq;
using Xunit;

namespace BervProject.WebApi.Test
{
    public class WeatherForecastTest
    {
        [Fact]
        public void GetTest()
        {
            var telemetryConfig = new TelemetryConfiguration("");
            var telemetryClient = new TelemetryClient(telemetryConfig);
            var controller = new WeatherForecastController(telemetryClient);

            var result = controller.Get();

            Assert.Equal(5, result.Count());
        }
    }
}
