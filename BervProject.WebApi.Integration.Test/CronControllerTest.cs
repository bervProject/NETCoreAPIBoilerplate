using BervProject.WebApi.Integration.Test.Fixtures;
using Hangfire;
using Microsoft.AspNetCore.Mvc.Testing;

namespace BervProject.WebApi.Integration.Test
{
    [Collection("Webapp")]
    public class CronControllerTest : IDisposable
    {
        private readonly WebApplicationFactory<Program> _applicationFactory;
        private readonly List<string> _registeredRecurring = new List<string>();
        public CronControllerTest(WebAppFixture webAppFixtures)
        {
            this._applicationFactory = webAppFixtures.WebApp;
        }

        public void Dispose()
        {
            RemoveRecurringJob();
        }

        private void RemoveRecurringJob()
        {
            var cronClient = (IRecurringJobManager?)this._applicationFactory.Services.GetService(typeof(IRecurringJobManager));
            if (cronClient != null)
            {
                foreach (var cronId in _registeredRecurring)
                {
                    cronClient.RemoveIfExists(cronId);
                }
            }
        }

        [Fact]
        public async Task SuccessCreateCronOnceTest()
        {
            var client = _applicationFactory.CreateClient();
            var response = await client.PostAsync("/api/v1.0/cron/CreateCronOnce", null);
            Assert.True(response.IsSuccessStatusCode);
            var stringResponse = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(stringResponse);
            var cronClient = (IBackgroundJobClient?)this._applicationFactory.Services.GetService(typeof(IBackgroundJobClient));
            if (cronClient != null)
            {
                var deleted = cronClient.Delete(stringResponse);
                Assert.True(deleted);
            }
        }

        [Fact]
        public async Task SuccessCreateRecuranceTest()
        {
            var client = _applicationFactory.CreateClient();
            var response = await client.PostAsync("/api/v1.0/cron/CreateRecurance", null);
            Assert.True(response.IsSuccessStatusCode);
            var stringResponse = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(stringResponse);
            _registeredRecurring.Add(stringResponse);
        }
    }
}