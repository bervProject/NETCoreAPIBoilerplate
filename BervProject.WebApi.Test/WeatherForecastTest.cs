using Autofac.Extras.Moq;
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
            using (var mock = AutoMock.GetLoose())
            {
                var controller = mock.Create<WeatherForecastController>();
                var result = controller.Get();
                Assert.Equal(5, result.Count());
            }
        }
    }
}
