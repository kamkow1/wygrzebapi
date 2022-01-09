using System.Net;
using System.Net.Mail;

namespace wygrzebapi.Email
{
    public class EmailService : IEmailService
    {
        public void Send(string to, string subject, string email, string password, string body)
        {
            using (MailMessage message = new(email, to))
            {
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;

                using (SmtpClient smtp = new())
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential cred = new(email, password);
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = cred;
                    smtp.Port = 587;

                    smtp.Send(message);
                }
            }
        }
    }
}
