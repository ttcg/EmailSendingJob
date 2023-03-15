using EmailSendingJob.EmailService;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.IO;

namespace EmailSendingJob
{
    public class SendMailByBlob
    {
        private readonly IEmailService _emailService;

        public SendMailByBlob(IEmailService emailService)
        {
            _emailService = emailService;
        }


        [FunctionName("SendMailByBlob")]
        public void Run([BlobTrigger("blobs/email/{name}", Connection = "AzureWebJobsStorage")]Stream myBlob, string name, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");

            using var reader = new StreamReader(myBlob);

            while (!reader.EndOfStream)
            {
                var email = reader.ReadLine();
                if (EmailValidator.IsValidEmailAddress(email))
                {
                    _emailService.SendEmail(email);
                }
                else
                {
                    log.LogWarning($"Email address: {email} is not a valid email address");
                }
            }
        }
    }
}
