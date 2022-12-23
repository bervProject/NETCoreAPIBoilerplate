using BervProject.WebApi.Boilerplate.Services;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Http;

namespace BervProject.WebApi.Boilerplate.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class CronController : ControllerBase
    {
        /// <summary>
        /// Create Cron Once
        /// </summary>
        /// <param name="backgroundJobClient"></param>
        /// <returns></returns>
        [HttpPost("CreateCronOnce")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]

        public IActionResult CreateCronOnce([FromServices] IBackgroundJobClient backgroundJobClient)
        {
            var jobId = backgroundJobClient.Enqueue<ICronService>((x) => x.HelloWorld());
            return Ok(jobId);
        }

        /// <summary>
        /// Create Cron Recurance
        /// </summary>
        /// <param name="recurringJobManager"></param>
        /// <returns></returns>
        [HttpPost("CreateRecurance")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult CreateRecurance([FromServices] IRecurringJobManager recurringJobManager)
        {
            var jobId = "HelloWorld";
            recurringJobManager.AddOrUpdate<ICronService>(jobId, (x) => x.HelloWorld(), Cron.Hourly);
            return Ok(jobId);
        }
    }
}
