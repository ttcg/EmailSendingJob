using EmailSendingJob.EmailService;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace EmailSendingJob
{
    public class SendMailByQueue
    {
        private readonly IEmailService _emailService;

        public SendMailByQueue(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [FunctionName("SendMailByQueue")]
        public void Run([QueueTrigger("email", Connection = "AzureWebJobsStorage")] string myQueueItem, ILogger log)
        {
            var emailMessage = JsonSerializer.Deserialize<SendEmailMessage>(myQueueItem,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

            if (EmailValidator.IsValidEmailAddress(emailMessage.Email))
            {
                _emailService.SendEmail(emailMessage.Email);
            }
            else
            {
                log.LogWarning($"Email address: {emailMessage.Email} is not a valid email address");
            }
        }

        private class SendEmailMessage
        {
            public string Email { get; set; }
        }
    }
}
