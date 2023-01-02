using BervProject.WebApi.Boilerplate.Models;
using BervProject.WebApi.Boilerplate.Models.Response;
using BervProject.WebApi.Boilerplate.Services.Azure;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BervProject.WebApi.Boilerplate.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ServiceBusSenderController : ControllerBase
    {

        /// <summary>
        /// Send Service Bus Message
        /// </summary>
        /// <param name="queueServices"></param>
        /// <param name="messageData"></param>
        /// <returns></returns>
        [HttpPost("sendMessage")]
        [ProducesResponseType(typeof(MessageSenderResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<MessageSenderResponse>> SendMessage([FromServices] IAzureQueueServices queueServices, [FromBody] MessageData messageData)
        {
            var response = new MessageSenderResponse()
            {
                YourMessage = messageData.Message
            };
            var result = await queueServices.SendMessage(messageData.Message);
            response.IsSuccess = result;
            return response;
        }

        /// <summary>
        /// Send Topic Message
        /// </summary>
        /// <param name="topicServices"></param>
        /// <param name="messageData"></param>
        /// <returns></returns>
        [HttpPost("sendTopic")]
        [ProducesResponseType(typeof(MessageSenderResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<MessageSenderResponse>> SendTopic([FromServices] ITopicServices topicServices, [FromBody] MessageData messageData)
        {
            var response = new MessageSenderResponse()
            {
                YourMessage = messageData.Message
            };
            var result = await topicServices.SendTopic(messageData.Message);
            response.IsSuccess = result;
            return response;
        }
    }
}
