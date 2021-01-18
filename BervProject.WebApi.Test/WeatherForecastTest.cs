using BervProject.WebApi.Boilerplate.Controllers;
using System.Linq;
using Xunit;

namespace BervProject.WebApi.Test
{
    public class WeatherForecastTest
    {
        [Fact]
        public void GetTest()
        {
            var controller = new WeatherForecastController();

            var result = controller.Get();

            Assert.Equal(5, result.Count());
        }
    }
}
