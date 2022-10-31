using System.Net.Http.Json;
using BervProject.WebApi.Integration.Test.Fixtures;
using BervProject.WebApi.Boilerplate.Entities;
using Microsoft.AspNetCore.Mvc.Testing;

namespace BervProject.WebApi.Integration.Test
{
    [Collection("Webapp")]
    public class NoteControllerTest
    {
        private readonly WebApplicationFactory<Program> _applicationFactory;
        public NoteControllerTest(WebAppFixture webAppFixtures)
        {
            this._applicationFactory = webAppFixtures.WebApp;
        }
        [Fact]
        public async Task CreateNoteTest()
        {
            var client = _applicationFactory.CreateClient();
            var response = await client.PostAsync("/api/v1.0/note/createTable", null);
            Assert.True(response.IsSuccessStatusCode);
            var stringResponse = await response.Content.ReadAsStringAsync();
            Assert.Equal("true", stringResponse);
            var partitionKey = "part-1";
            var rowKey = "row-1";
            var title = "Hello World!";
            var message = "Yes!";
            var newNote = new Note()
            {
                PartitionKey = partitionKey,
                RowKey = rowKey,
                Title = title,
                Message = message,
            };
            response = await client.PostAsJsonAsync<Note>("/api/v1.0/note/upsert", newNote);
            Assert.True(response.IsSuccessStatusCode);
            stringResponse = await response.Content.ReadAsStringAsync();
            Assert.Equal("true", stringResponse);
            response = await client.GetAsync($"/api/v1.0/note/get?partitionKey={partitionKey}&rowKey={rowKey}");
            Assert.True(response.IsSuccessStatusCode);
            var data = await response.Content.ReadFromJsonAsync<Note>();
            Assert.NotNull(data);
            Assert.Equal(title, data.Title);
            Assert.Equal(message, data.Message);
        }
    }
}