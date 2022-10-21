using Microsoft.AspNetCore.Mvc.Testing;

namespace BervProject.WebApi.Integration.Test.Fixtures
{
    public class WebAppFixture : IDisposable
    {
        public WebAppFixture()
        {
            WebApp = new WebApplicationFactory<Program>()
                    .WithWebHostBuilder(builder =>
                    {
                        // ... Configure test services
                    });

        }

        public void Dispose() => WebApp.Dispose();

        public WebApplicationFactory<Program> WebApp { get; private set; }
    }
}
