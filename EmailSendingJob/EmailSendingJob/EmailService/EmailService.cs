using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Mail;

namespace EmailSendingJob.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly EmailSetting _emailSetting;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<EmailSetting> emailSetting, ILogger<EmailService> logger)
        {
            _emailSetting = emailSetting.Value;
            _logger = logger;
        }
        public void SendEmail(string email)
        {
            if(!Validate())
            {
                return;
            }

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(
                                    _emailSetting.EmailUserName,
                                    _emailSetting.Password),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailSetting.From),
                Subject = "Test mail from Azure Function",
                Body = $"Test mail sent at: {DateTime.UtcNow.ToString("o")}"
            };

            mailMessage.To.Add(email);

            smtpClient.Send(mailMessage);

            _logger.LogInformation($"Test Email sent to {email}");
        }

        private bool Validate()
        {
            var isValid = true;
            if (string.IsNullOrWhiteSpace(_emailSetting.EmailUserName))
            {
                _logger.LogError("Email Username is not configured.");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(_emailSetting.Password))
            {
                _logger.LogError("Email Password is not configured.");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(_emailSetting.From))
            {
                _logger.LogError("From Email Address is not configured.");
                isValid = false;
            }



            return isValid;
        }
    }
}
