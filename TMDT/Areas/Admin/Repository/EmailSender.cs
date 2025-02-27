using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace TMDT.Areas.Admin.Repository
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true, //bật bảo mật
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("webdemo3108@gmail.com", "mznm ifje mjjr jeuw")
            };

            return client.SendMailAsync(
                new MailMessage(from: "webdemo3108@gmail.com",
                                to: email,
                                subject,
                                message
                                ));
        }
    }
}
