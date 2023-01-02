using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using BervProject.WebApi.Boilerplate.Models.Request;
using BervProject.WebApi.Boilerplate.Services.Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BervProject.WebApi.Boilerplate.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class BlobController : ControllerBase
    {
        private readonly IBlobService _blobService;
        private readonly ILogger<BlobController> _logger;

        /// <summary>
        /// Blob Controller Constructor
        /// </summary>
        /// <param name="blobService"></param>
        /// <param name="logger"></param>
        public BlobController(IBlobService blobService, ILogger<BlobController> logger)
        {
            _blobService = blobService;
            _logger = logger;
        }

        /// <summary>
        /// Create Blob Container
        /// </summary>
        /// <returns>true/false</returns>
        [HttpPost("create")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public IActionResult CreateBlobContainer()
        {
            _blobService.CreateStorageContainer();
            return Ok(true);
        }

        /// <summary>
        /// Return list of blob
        /// </summary>
        /// <returns></returns>
        [HttpGet("list")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<Dictionary<string, string>>), StatusCodes.Status200OK)]
        public IActionResult ListBlob()
        {
            var info = _blobService.GetBlobsInfo();
            return Ok(info);
        }

        /// <summary>
        /// Upload Blob
        /// </summary>
        /// <param name="uploadFile"></param>
        /// <returns></returns>
        [HttpPost("upload")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public IActionResult Upload([FromForm] UploadFile uploadFile)
        {
            _blobService.UploadFile(uploadFile.File);
            return Ok(true);
        }
    }
}
