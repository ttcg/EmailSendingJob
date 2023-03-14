using EmailSendingJob.EmailService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

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

            var toEmail = req.Query["to"];

            var isValid = EmailValidator.IsValidEmailAddress(toEmail);

            string responseMessage = string.IsNullOrWhiteSpace(toEmail)
                ? "Please pass a mandatory email address - 'to' in the query string or in the request body."
                : $"Test Email sent to {toEmail}";

            if (isValid)
            {
                _emailService.SendEmail(toEmail);
            }            

            return new OkObjectResult(responseMessage);
        }
    }
}
