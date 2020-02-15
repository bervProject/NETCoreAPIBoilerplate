using BervProject.WebApi.Boilerplate.Models;
using BervProject.WebApi.Boilerplate.Services;
using Microsoft.AspNetCore.Mvc;

namespace BervProject.WebApi.Boilerplate.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send")]
        public void SendEmail([FromBody] EmailSendRequest request)
        {
            _emailService.SendEmail(request.To);
        }
    }
}
