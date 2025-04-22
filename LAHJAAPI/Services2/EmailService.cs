using LAHJAAPI.Models;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MimeKit;
using LAHJAAPI.Utilities;


namespace LAHJAAPI.Utilities.Services2
{

    public interface IEmailService : IEmailSender<ApplicationUser>
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage);
        Task SendSubscriptionSuccessEmailAsync(string recipientEmail, string recipientName, string planName, string duration, string activationDate, string subscriptionId, IList<IFormFile>? formFiles = null);
    }
    public class EmailService(IOptions<SmtpConfig> options) : IEmailService

    {
        private SmtpConfig SmtpConfig => options.Value;

        public async Task SendSubscriptionSuccessEmailAsync(string recipientEmail, string recipientName, string planName, string duration, string activationDate, string subscriptionId, IList<IFormFile>? formFiles = null)
        {
            // قراءة قالب HTML
            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "EmailTemplates", "SubscriptionSuccessTemplate.html");
            string emailBody = await File.ReadAllTextAsync(templatePath);

            // استبدال القيم الديناميكية
            emailBody = emailBody.Replace("[Client Name]", recipientName)
                                 .Replace("[Plan Name]", planName)
                                 .Replace("[Duration]", duration)
                                 .Replace("[Activation Date]", activationDate)
                                 .Replace("[Subscription Number]", subscriptionId)
                                 .Replace("[Website link]", "https://localhost:7584"); // ضع رابط موقعك

            // إعداد الرسالة
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("ASG", SmtpConfig.FromEmail));
            message.To.Add(new MailboxAddress(recipientName, recipientEmail));
            message.Subject = "Your subscription has been successfully activated 🎉";

            // إعداد محتوى HTML
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = emailBody
            };
            if (formFiles != null)
            {
                foreach (var file in formFiles)
                {
                    if (file.Length > 0)
                    {
                        using var ms = new MemoryStream();
                        await file.CopyToAsync(ms);
                        var fileBytes = ms.ToArray();
                        bodyBuilder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }
            message.Body = bodyBuilder.ToMessageBody();

            // إرسال البريد الإلكتروني
            using var smtpClient = new SmtpClient();
            try
            {
                await smtpClient.ConnectAsync(SmtpConfig.Host, SmtpConfig.Port, MailKit.Security.SecureSocketOptions.StartTls); // تعديل إعدادات SMTP حسب مزود البريد الإلكتروني
                await smtpClient.AuthenticateAsync(SmtpConfig.UserName, SmtpConfig.Password); // بيانات البريد الخاص بك
                await smtpClient.SendAsync(message);
            }
            finally
            {
                await smtpClient.DisconnectAsync(true);
            }
        }

        public Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink)
        {
            return SendEmailAsync(email, "Confirm your email for MyWebSite", $"Please confirm your MyWebSite account by <a href='{confirmationLink}'>clicking here</a>.");

        }

        public Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink)
        {
            return SendEmailAsync(email, "Reset your password for MyWebSite", $"Please reset your MyWebSite password by <a href='{resetLink}'>clicking here</a>.");

        }

        public Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode)
        {
            // إعداد الرسالة
            return SendEmailAsync(email, "Reset your password for ASG", $"Reset your MyWebSite password using the following code: {resetCode}");
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("ASG", SmtpConfig.FromEmail));
            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = htmlMessage
            };

            message.Body = bodyBuilder.ToMessageBody();
            using var smtpClient = new SmtpClient();
            try
            {
                smtpClient.Connect(SmtpConfig.Host, SmtpConfig.Port, MailKit.Security.SecureSocketOptions.StartTls); // تعديل إعدادات SMTP حسب مزود البريد الإلكتروني
                smtpClient.Authenticate(SmtpConfig.UserName, SmtpConfig.Password); // بيانات البريد الخاص بك
                smtpClient.Send(message);
            }
            finally
            {
                smtpClient.Disconnect(true);
            }

            return Task.CompletedTask;
        }
    }


}
