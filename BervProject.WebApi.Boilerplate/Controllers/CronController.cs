using BervProject.WebApi.Boilerplate.Services;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BervProject.WebApi.Boilerplate.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class CronController : ControllerBase
    {
        [HttpPost("CreateCronOnce")]
        public IActionResult CreateCronOnce([FromServices] IBackgroundJobClient backgroundJobClient)
        {
            var jobId = backgroundJobClient.Enqueue<ICronService>((x) => x.HelloWorld());
            return Ok(jobId);
        }

        [HttpPost("CreateRecurance")]
        public IActionResult CreateRecurance([FromServices] IRecurringJobManager recurringJobManager)
        {
            var jobId = "HelloWorld";
            recurringJobManager.AddOrUpdate<ICronService>(jobId, (x) => x.HelloWorld(), Cron.Hourly);
            return Ok(jobId);
        }
    }
}
