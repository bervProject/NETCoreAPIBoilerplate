using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using BervProject.WebApi.Boilerplate.ConfigModel;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace BervProject.WebApi.Boilerplate.Services
{
    public class DynamoDbServices : IDynamoDbServices
    {
        private readonly AmazonDynamoDBClient _dynamoClient;
        private readonly ILogger<DynamoDbServices> _logger;
        public DynamoDbServices(AWSConfiguration awsConfiguration, ILogger<DynamoDbServices> logger)
        {
            _logger = logger;
            var credential = new BasicAWSCredentials(awsConfiguration.Basic.Auth.AccessKey, awsConfiguration.Basic.Auth.SecretKey);
            _dynamoClient = new AmazonDynamoDBClient(credential, RegionEndpoint.GetBySystemName(awsConfiguration.Dynamo.Location));
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
