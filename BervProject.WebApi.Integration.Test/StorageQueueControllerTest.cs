namespace BervProject.WebApi.Integration.Test;

using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Boilerplate.Models;
using Boilerplate.Models.Response;
using Fixtures;

[Collection("Webapp")]
public class StorageQueueControllerTest
{
    private readonly WebAppFixture _fixture;

    public StorageQueueControllerTest(WebAppFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task StorageQueueSendMessageTest()
    {
        var client = _fixture.CreateApiClient();
        var messageData = new MessageData
        {
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
