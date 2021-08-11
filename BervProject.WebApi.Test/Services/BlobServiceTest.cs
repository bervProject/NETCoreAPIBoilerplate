using Autofac;
using Autofac.Extras.Moq;
using Azure.Storage.Blobs;
using BervProject.WebApi.Boilerplate.ConfigModel;
using BervProject.WebApi.Boilerplate.Services.Azure;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BervProject.WebApi.Test.Services
{
    public class BlobServiceTest
    {
        [Fact]
        public void CreateStorageTest()
        {
            var azureConfig = new AzureConfiguration
            {
                Storage = new AzureStorage
                {
                    Blob = new BlobStorage
                    {
                        ContainerName = ""
                    }
                }
            };
            using var mock = AutoMock.GetLoose(cfg => cfg.RegisterInstance(azureConfig).As<AzureConfiguration>());
            var loggerMock = mock.Mock<ILogger<BlobService>>();
            var blobServiceClient = mock.Mock<BlobServiceClient>();
            var mockContainer = mock.Mock<BlobContainerClient>();
            blobServiceClient.Setup(x => x.GetBlobContainerClient("")).Returns(mockContainer.Object);
            var blobService = mock.Create<BlobService>();
            blobService.CreateStorageContainer();
            mockContainer.Verify(x => x.CreateIfNotExists(Azure.Storage.Blobs.Models.PublicAccessType.None, null, null, default), Times.Once());
        }
    }
}
