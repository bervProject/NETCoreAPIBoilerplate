using Azure.Storage.Blobs;
using BervProject.WebApi.Boilerplate.ConfigModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace BervProject.WebApi.Boilerplate.Services.Azure
{
    public class BlobService : IBlobService
    {
        private readonly ILogger<BlobService> _logger;
        private readonly BlobContainerClient _blobContainerClient;
        private readonly string _containerName;
        public BlobService(ILogger<BlobService> logger, AzureConfiguration azureConfiguration, BlobServiceClient blobServiceClient)
        {
            _logger = logger;
            _containerName = azureConfiguration.Storage.Blob.ContainerName;
            _blobContainerClient = blobServiceClient.GetBlobContainerClient(_containerName);
        }

        public void CreateStorageContainer()
        {
            _logger.LogDebug($"Create Blob Container {_containerName}");
            _blobContainerClient.CreateIfNotExists();
            _logger.LogDebug($"Blob Container {_containerName} created");
        }

        public List<Dictionary<string, string>> GetBlobsInfo()
        {
            var list = new List<Dictionary<string, string>>();
            if (_blobContainerClient.Exists().Value)
            {
                var blobs = _blobContainerClient.GetBlobs();
                foreach (var blob in blobs)
                {
                    _logger.LogDebug($"{blob.Name} --> Created On: {blob.Properties.CreatedOn:YYYY-MM-dd HH:mm:ss}  Size: {blob.Properties.ContentLength}");
                    list.Add(new Dictionary<string, string>
                    {
                        { "name", blob.Name },
                        { "createdDate", blob.Properties.CreatedOn?.ToString() },
                        { "size", blob.Properties.ContentLength?.ToString() },
                        { "version", blob.VersionId },
                        { "deleted", blob.Deleted.ToString() }
                    });
                }
            }
            else
            {
                _logger.LogWarning($"Can't get data, container {_containerName} not created yet");
            }
            return list;
        }

        public void UploadFile(IFormFile formFile)
        {
            if (_blobContainerClient.Exists().Value)
            {
                var fileName = formFile.FileName;
                using (var stream = formFile.OpenReadStream())
                {
                    _logger.LogDebug($"Uploading {fileName}");
                    _blobContainerClient.UploadBlob(fileName, stream);
                }
                _logger.LogDebug($"{fileName} uploaded");
            }
            else
            {
                _logger.LogWarning($"Can't upload, container {_containerName} not created yet");
            }
        }
    }
}
