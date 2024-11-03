using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public class EmailSender : IEmailSender
    {
        public string SendGrid;

        public EmailSender(IConfiguration _config)
        {
            SendGrid = _config.GetValue<string>("SendGrid:secretKey");
        }

        Task IEmailSender.SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SendGridClient(SendGrid);
            var from = new EmailAddress("Admin@befranchisor.com", "ConfarmPassword1");
            var to = new EmailAddress(email);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlMessage);
            return client.SendEmailAsync(msg);
        }
    }
}
