using BervProject.WebApi.Boilerplate.Models;
using BervProject.WebApi.Boilerplate.Models.Response;
using BervProject.WebApi.Boilerplate.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BervProject.WebApi.Boilerplate.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ServiceBusSenderController : ControllerBase
    {
        private readonly IServiceBusServices _serviceBusServices;
        public ServiceBusSenderController(IServiceBusServices serviceBusServices)
        {
            _serviceBusServices = serviceBusServices;
        }

        [HttpPost("sendMessage")]
        public async Task<ActionResult<MessageSenderResponse>> SendMessage([FromBody] MessageData messageData)
        {
            var response = new MessageSenderResponse()
            {
                YourMessage = messageData.Message
            };
            var result = await _serviceBusServices.SendMessage(messageData.Message);
            response.IsSuccess = result;
            return response;
        }
    }
}
