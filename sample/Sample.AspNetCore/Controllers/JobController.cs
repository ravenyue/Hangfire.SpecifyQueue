using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.SpecifyQueue;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sample.AspNetCore.Services;

namespace Sample.AspNetCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JobController : ControllerBase
    {
        private readonly ILogger<JobController> _logger;
        private readonly IBackgroundJobClient _jobClient;

        public JobController(
            ILogger<JobController> logger,
            IBackgroundJobClient jobClient)
        {
            _logger = logger;
            _jobClient = jobClient;
        }

        [HttpGet("Enqueue")]
        public IActionResult Enqueue()
        {
            var jobid = _jobClient.Enqueue<IHelloService>(queueName: "hello", x => x.Hello("jack"));

            return Ok(jobid);
        }

        [HttpGet("Delay")]
        public IActionResult Delay()
        {
            var jobid = _jobClient.Schedule<IHelloService>(
                        queueName: "hello",
                        methodCall: x => x.Hello("jack"),
                        delay: TimeSpan.FromSeconds(5));

            return Ok(jobid);
        }

        [HttpGet("Failed")]
        public IActionResult Failed()
        {
            var jobid = _jobClient.Enqueue<IHelloService>(queueName: "hello", x => x.Failed());

            return Ok(jobid);
        }
    }
}
