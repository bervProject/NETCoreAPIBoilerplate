using BervProject.WebApi.Boilerplate.Models;
using BervProject.WebApi.Boilerplate.Services.AWS;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BervProject.WebApi.Boilerplate.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendEmail([FromBody] EmailSendRequest request)
        {
            await _emailService.SendEmail(request.To);
            return Ok(new
            {
                Status = 200
            });
        }
    }
}
