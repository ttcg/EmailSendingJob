using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EmailSendingJob
{
    public class SendTestMail
    {
        private readonly IEmailService _emailService;

        public SendTestMail(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [FunctionName("SendTestMail")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Executing SendTestMail");

            _emailService.SendEmail("hello");

            return new OkObjectResult("successfully executed");
        }
    }
}
