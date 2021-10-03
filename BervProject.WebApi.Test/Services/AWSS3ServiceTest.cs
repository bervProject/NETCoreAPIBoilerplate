using Amazon.S3;
using Amazon.S3.Model;
using Autofac.Extras.Moq;
using BervProject.WebApi.Boilerplate.Services.AWS;
using Microsoft.AspNetCore.Http;
using Moq;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace BervProject.WebApi.Test.Services
{
    public class AWSS3ServiceTest
    {

        [Fact]
        public async Task UploadFileSuccess()
        {
            using var mock = AutoMock.GetLoose();
            var amazonS3Mock = mock.Mock<IAmazonS3>();
            var fileMock = mock.Mock<IFormFile>();
            var nullStream = new MemoryStream(0);
            var fileName = "ok.jpg";
            var contentType = "image/jpeg";
            amazonS3Mock.Setup(x => x.PutObjectAsync(It.IsAny<PutObjectRequest>(), default));
            fileMock.SetupGet(x => x.FileName).Returns(fileName);
            fileMock.SetupGet(x => x.ContentType).Returns(contentType);
            fileMock.Setup(x => x.OpenReadStream()).Returns(nullStream);
            var awsS3Service = mock.Create<AWSS3Service>();
            var result = await awsS3Service.UploadFile(fileMock.Object);
            Assert.Equal($"uploads/{fileName}", result);
            fileMock.Verify(x => x.OpenReadStream(), Times.Once());
            amazonS3Mock.Verify(x => x.PutObjectAsync(It.IsAny<PutObjectRequest>(), default), Times.Once());
        }
    }
}
