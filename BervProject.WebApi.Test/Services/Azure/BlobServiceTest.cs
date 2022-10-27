using Autofac;
using Autofac.Extras.Moq;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BervProject.WebApi.Boilerplate.ConfigModel;
using BervProject.WebApi.Boilerplate.Services.Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System.IO;
using Xunit;

namespace BervProject.WebApi.Test.Services.Azure
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
            mockContainer.Verify(x => x.CreateIfNotExists(PublicAccessType.None, null, null, default), Times.Once());
        }

        [Fact]
        public void GettingEmptyBlobWhenContainerNotExists()
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
            mockContainer.Setup(x => x.Exists(default).Value).Returns(false);
            var blobService = mock.Create<BlobService>();
            var listResult = blobService.GetBlobsInfo();
            mockContainer.Verify(x => x.Exists(default), Times.Once());
            Assert.Empty(listResult);
        }

        [Fact]
        public void UploadFileWhenContainerExist()
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
            var mockBlobClient = mock.Mock<BlobClient>();
            var fileMock = mock.Mock<IFormFile>();
            var nullStream = new MemoryStream(0);
            blobServiceClient.Setup(x => x.GetBlobContainerClient("")).Returns(mockContainer.Object);
            mockContainer.Setup(x => x.Exists(default).Value).Returns(true);
            var fileName = "Dummy";
            mockContainer.Setup(x => x.GetBlobClient(fileName)).Returns(mockBlobClient.Object);
            fileMock.SetupGet(x => x.FileName).Returns(fileName);
            fileMock.Setup(x => x.OpenReadStream()).Returns(nullStream);
            var blobService = mock.Create<BlobService>();
            blobService.UploadFile(fileMock.Object);
            fileMock.Verify(x => x.OpenReadStream(), Times.Once());
            mockContainer.Verify(x => x.GetBlobClient(fileName), Times.Once());
            mockBlobClient.Verify(x => x.Upload(It.IsAny<Stream>(), true, default), Times.Once());
        }

        [Fact]
        public void UploadFileWhenContainerNotExist()
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
            var fileMock = mock.Mock<IFormFile>();
            var nullStream = new MemoryStream(0);
            blobServiceClient.Setup(x => x.GetBlobContainerClient("")).Returns(mockContainer.Object);
            mockContainer.Setup(x => x.Exists(default).Value).Returns(false);
            var fileName = "Dummy";
            fileMock.SetupGet(x => x.FileName).Returns(fileName);
            fileMock.Setup(x => x.OpenReadStream()).Returns(nullStream);
            var blobService = mock.Create<BlobService>();
            blobService.UploadFile(fileMock.Object);
            fileMock.Verify(x => x.OpenReadStream(), Times.Never());
            mockContainer.Verify(x => x.UploadBlob(fileName, nullStream, default), Times.Never());
        }
    }
}
