using System.Net.Mail;

namespace LeaveManagementSystem.Web.Services
{
    // IConfiguration: access to configuration settings in appsettings.json
    // IEmailSender: default interface provided by ASP.NET Core Identity
    public class EmailSender(IConfiguration _configuration) : IEmailSender 
    {

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // ["EmailSettings:DefaultEmailAddress"]: key: appsettings.json > EmailSettings > DefaultEmailAddress
            var fromAddress = _configuration["EmailSettings:DefaultEmailAddress"];
            var smtpServer = _configuration["EmailSettings:Server"];
            var smtpPort = Convert.ToInt32(_configuration["EmailSettings:Port"]);

            var message = new MailMessage
            {
                From = new MailAddress(fromAddress!),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true, // allow HTML content in the email body
            };

            message.To.Add(new MailAddress(email));

            using var client = new SmtpClient(smtpServer, smtpPort);
            await client.SendMailAsync(message);
        }
    }
}
