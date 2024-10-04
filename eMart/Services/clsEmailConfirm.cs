using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace eMart.Services
{
    public class clsEmailConfirm : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var fEmail = "alitalaat142@outlook.com";
            var fPassword = "1952001_Ali";

            var theMsg = new MailMessage();
            theMsg.From = new MailAddress(fEmail);
            theMsg.Subject = subject;
            theMsg.To.Add(email);
            theMsg.Body = $"<html><body>{htmlMessage}</body></html>";
            theMsg.IsBodyHtml = true;

            var smtpClint = new SmtpClient("smtp.office365.com")
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(fEmail, fPassword),
                Port = 587
            };

          smtpClint.Send(theMsg);
        }
    }
}

