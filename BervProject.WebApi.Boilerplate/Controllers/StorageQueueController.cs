using BervProject.WebApi.Boilerplate.Models;
using BervProject.WebApi.Boilerplate.Models.Response;
using BervProject.WebApi.Boilerplate.Services.Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BervProject.WebApi.Boilerplate.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class StorageQueueController : ControllerBase
    {
        private readonly IAzureStorageQueueService _azureStorageQueueService;
        public StorageQueueController(IAzureStorageQueueService azureStorageQueueService)
        {
            _azureStorageQueueService = azureStorageQueueService;
        }

        /// <summary>
        /// Send Queue Message
        /// </summary>
        /// <param name="messageData"></param>
        /// <returns></returns>
        [HttpPost("sendMessage")]
        [ProducesResponseType(typeof(MessageSenderResponse), StatusCodes.Status200OK)]
        public ActionResult<MessageSenderResponse> SendMessage([FromBody] MessageData messageData)
        {
            var response = new MessageSenderResponse()
            {
                YourMessage = messageData.Message
            };
            var result = _azureStorageQueueService.SendMessage(messageData.Message);
            response.IsSuccess = result;
            return response;
        }

        /// <summary>
        /// Getting Latest Message
        /// </summary>
        /// <returns></returns>
        [HttpGet("receiveMessage")]
        [ProducesResponseType(typeof(MessageSenderResponse), StatusCodes.Status200OK)]
        public ActionResult<MessageSenderResponse> GetLatestMessage()
        {
            var response = new MessageSenderResponse();
            var result = _azureStorageQueueService.ReceiveMessage();
            response.IsSuccess = result != null;
            response.YourMessage = result;
            return response;
        }
    }
}
