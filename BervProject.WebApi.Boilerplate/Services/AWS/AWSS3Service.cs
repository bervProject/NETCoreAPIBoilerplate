using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Amazon.S3;
using Amazon.S3.Model;

namespace BervProject.WebApi.Boilerplate.Services.AWS
{
    public class AWSS3Service : IAWSS3Service
    {
        private IAmazonS3 _s3Client;
        public AWSS3Service(IAmazonS3 amazonS3)
        {
            _s3Client = amazonS3;
        }
        public async Task<string> UploadFile(IFormFile formFile)
        {
            var location = $"uploads/{formFile.FileName}";
            using (var stream = formFile.OpenReadStream())
            {
                var putRequest = new PutObjectRequest
                {
                    Key = location,
                    BucketName = "upload-test-berv",
                    InputStream = stream,
                    AutoCloseStream = true,
                    ContentType = formFile.ContentType
                };
                await _s3Client.PutObjectAsync(putRequest);
                return location;
            }
        }
    }
}
