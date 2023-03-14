using System.Net.Mail;

namespace EmailSendingJob
{
    public static class EmailValidator
    {
        public static bool IsValidEmailAddress(string email)
        {
            var valid = true;

            try
            {
                var emailAddress = new MailAddress(email);
            }
            catch
            {
                valid = false;
            }

            return valid;
        }
    }
}
