using System;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Autofac.Extras.Moq;
using BervProject.WebApi.Boilerplate.Services.AWS;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BervProject.WebApi.Test.Services.AWS
{
    public class DynamoDbServicesTest
    {
        [Fact]
        public async Task Test_CreateObject()
        {
            using var mock = AutoMock.GetLoose();
            var amazonDynamoDBMock = mock.Mock<IAmazonDynamoDB>();
            var logMock = mock.Mock<ILogger<DynamoDbServices>>();
            amazonDynamoDBMock.Setup(x => x.PutItemAsync(It.IsAny<PutItemRequest>(), default))
              .Returns(Task.FromResult(new PutItemResponse
              {
                  HttpStatusCode = System.Net.HttpStatusCode.OK,
                  Attributes = new System.Collections.Generic.Dictionary<string, AttributeValue>
                {
                  {"mock", new AttributeValue ("Hello")}
                }
              }));
            var dynamoDbServices = mock.Create<DynamoDbServices>();
            await dynamoDbServices.CreateObject();
            amazonDynamoDBMock.Verify(x => x.PutItemAsync(It.IsAny<PutItemRequest>(), default), Times.Once());
        }
    }
}
