namespace BervProject.WebApi.Boilerplate.Services.AWS;

using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

/// <inheritdoc />
public class DynamoDbServices : IDynamoDbServices
{
    private readonly IAmazonDynamoDB _dynamoClient;
    private readonly ILogger<DynamoDbServices> _logger;
    /// <summary>
    /// Default constructor with dependency injections
    /// </summary>
    /// <param name="amazonDynamoDb"></param>
    /// <param name="logger"></param>
    public DynamoDbServices(IAmazonDynamoDB amazonDynamoDb, ILogger<DynamoDbServices> logger)
    {
        _logger = logger;
        _dynamoClient = amazonDynamoDb;
    }

    /// <summary>
    /// Create Object
    /// </summary>
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
        string message = $"Response: {response.HttpStatusCode}, {JsonSerializer.Serialize(response.Attributes)}";
        _logger.LogInformation(message);
    }
}