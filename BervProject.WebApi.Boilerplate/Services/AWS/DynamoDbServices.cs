using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using BervProject.WebApi.Boilerplate.ConfigModel;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace BervProject.WebApi.Boilerplate.Services.AWS
{
    public class DynamoDbServices : IDynamoDbServices
    {
        private readonly IAmazonDynamoDB _dynamoClient;
        private readonly ILogger<DynamoDbServices> _logger;
        public DynamoDbServices(IAmazonDynamoDB amazonDynamoDb, ILogger<DynamoDbServices> logger)
        {
            _logger = logger;
            _dynamoClient = amazonDynamoDb;
        }

        public async Task CreateObject()
        {
            var request = new PutItemRequest()
            {
                TableName = "dev-test",
                Item = new Dictionary<string, AttributeValue>
                {
                    { "Id", new AttributeValue
                    {
                        S = Guid.NewGuid().ToString()
                    }},
                    { "Name", new AttributeValue
                    {
                        S = "Hello World!"
                    }}
                }
            };
            var response = await _dynamoClient.PutItemAsync(request);
            _logger.LogInformation($"Response: {response.HttpStatusCode.ToString()}, {JsonSerializer.Serialize(response.Attributes)}");
        }
    }
}
