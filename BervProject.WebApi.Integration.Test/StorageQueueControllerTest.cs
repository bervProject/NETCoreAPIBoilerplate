using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using BervProject.WebApi.Boilerplate.Models;
using BervProject.WebApi.Boilerplate.Models.Response;
using BervProject.WebApi.Integration.Test.Fixtures;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json.Linq;

namespace BervProject.WebApi.Integration.Test
{
    [Collection("Webapp")]
    public class StorageQueueControllerTest
    {
        private readonly WebApplicationFactory<Program> _applicationFactory;
        public StorageQueueControllerTest(WebAppFixture webAppFixtures)
        {
            this._applicationFactory = webAppFixtures.WebApp;
        }
        [Fact]
        public async Task StorageQueueSendMessageTest()
        {
            var client = _applicationFactory.CreateClient();
            var messageData = new MessageData{
                Message = "Hello World!"
            };
            using var content = new StringContent(JsonSerializer.Serialize(messageData), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/v1.0/storagequeue/sendMessage", content);
            Assert.True(response.IsSuccessStatusCode);
            var data = await response.Content.ReadFromJsonAsync<MessageSenderResponse>();
            Assert.NotNull(data);
            Assert.True(data.IsSuccess);
            response = await client.GetAsync("/api/v1.0/storagequeue/receiveMessage");
            Assert.True(response.IsSuccessStatusCode);
            data = await response.Content.ReadFromJsonAsync<MessageSenderResponse>();
            Assert.NotNull(data);
            Assert.True(data.IsSuccess);
            Assert.Equal("Hello World!", data.YourMessage);

        }
    }
}
