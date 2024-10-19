
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;


namespace eMart.Services
{
    public class MailingService : IEmailSender
    {
        private readonly MailSettings _mailSettings;

        public MailingService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendEmailAsync(string mailTo, string subject, string htmlMessage)
        {
            var apiKey = _mailSettings.ApiKey;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(_mailSettings.Email, _mailSettings.DisplayName);
            var to = new EmailAddress(mailTo ,mailTo);
            var plainTextContent = "";
            var htmlContent = htmlMessage;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }


     
    }

}