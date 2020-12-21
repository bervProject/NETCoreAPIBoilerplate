using BervProject.WebApi.Boilerplate.Models;
using BervProject.WebApi.Boilerplate.Models.Response;
using BervProject.WebApi.Boilerplate.Services.Azure;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BervProject.WebApi.Boilerplate.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ServiceBusSenderController : ControllerBase
    {

        [HttpPost("sendMessage")]
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

        [HttpPost("sendTopic")]
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
