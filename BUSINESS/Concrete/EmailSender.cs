using ENTITIES.Entities;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUSINESS.Concrete
{
    public class EmailSender : IEmailSender
    {
       
        private readonly ILogger _logger;

        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor,
                           ILogger<EmailSender> logger)
        {
            Options = optionsAccessor.Value;
            _logger = logger;
        }

        public AuthMessageSenderOptions Options { get; } //Set with Secret Manager.

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            if (string.IsNullOrEmpty(Options.SendGridKey))
            {
                throw new Exception("Null SendGridKey");
            }
            await Execute(Options.SendGridKey, subject, message, toEmail);
        }

        public async Task Execute(string apiKey, string subject, string message, string toEmail)
        {
            var client = new SendGridClient(apiKey);


            var From = new EmailAddress("", "");
            var To=new EmailAddress(toEmail);
            var Subject = subject;
            var PlainTextContent = message;
            var HtmlContent = message;
            var msg = MailHelper.CreateSingleEmail(
                From,
                To,
                Subject,
                PlainTextContent,
                HtmlContent
                );

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);
            var response = await client.SendEmailAsync(msg);
            //logger'a bakılacak!!!!!
            //_logger.LogInformation(response.IsSuccessStatusCode
            //                       ? $"Email to {toEmail} queued successfully!"
            //                       : $"Failure Email to {toEmail}");
        }
    }
}
