namespace BervProject.WebApi.Integration.Test;

using System.Net.Http.Json;
using Fixtures;

[Collection("Webapp")]
public class BlobControllerTest
{
    private readonly WebAppFixture _fixture;

    public BlobControllerTest(WebAppFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task UploadBlobTest()
    {
        var client = _fixture.CreateApiClient();
        var response = await client.PostAsync("/api/v1.0/blob/create", null);
        Assert.True(response.IsSuccessStatusCode);
        using var file1 = File.OpenRead(@"Docs/test.txt");
        using var content1 = new StreamContent(file1);
        using var formData = new MultipartFormDataContent();
        formData.Add(content1, "file", "test.txt");
        response = await client.PostAsync("/api/v1.0/blob/upload", formData);
        Assert.True(response.IsSuccessStatusCode);
        response = await client.GetAsync("/api/v1.0/blob/list");
        Assert.True(response.IsSuccessStatusCode);
        var data = await response.Content.ReadFromJsonAsync<List<Dictionary<string, string>>>();
        Assert.NotNull(data);
        Assert.Single(data);
    }
}
