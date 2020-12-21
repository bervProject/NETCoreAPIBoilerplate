using BervProject.WebApi.Boilerplate.Models.Request;
using BervProject.WebApi.Boilerplate.Services.Azure;
using Microsoft.AspNetCore.Mvc;

namespace BervProject.WebApi.Boilerplate.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class BlobController : ControllerBase
    {
        private readonly IBlobService _blobService;
        public BlobController(IBlobService blobService)
        {
            _blobService = blobService;
        }

        [HttpPost("create")]
        public IActionResult CreateBlobContainer()
        {
            _blobService.CreateStorageContainer();
            return Ok(true);
        }

        [HttpGet("list")]
        public IActionResult ListBlob()
        {
            var info =_blobService.GetBlobsInfo();
            return Ok(info);
        }

        [HttpPost("upload")]
        public IActionResult Upload([FromForm] UploadFile uploadFile)
        {
            _blobService.UploadFile(uploadFile.File);
            return Ok(true);
        }
    }
}
