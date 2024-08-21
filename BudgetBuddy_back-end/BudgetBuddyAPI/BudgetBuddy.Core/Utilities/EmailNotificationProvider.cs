using BudgetBuddy.Core.AppSettings;
using BudgetBuddy.Core.Interface;
using MailKit.Net.Smtp;
using Microsoft.Extensions.DependencyInjection;
using MimeKit;
using MimeKit.Text;
using MailKit.Security;

namespace BudgetBuddy.Core.Utilities
{
    public class EmailNotificationProvider : INotificationProvider
    {
        private readonly NotificationSettings _notificationSettings;
        public EmailNotificationProvider(IServiceProvider provider)
        {
            _notificationSettings = provider.GetRequiredService<NotificationSettings>();
        }

        public async Task<bool> SendAsync(NotificationContext context)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_notificationSettings.From));
                email.To.Add(MailboxAddress.Parse(context.Address));
                email.Subject = context.Header;
                email.Body = new TextPart(TextFormat.Html)
                {
                    Text = context.Payload
                };
                using var smtp = new SmtpClient();
                smtp.Connect(_notificationSettings.Host, _notificationSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_notificationSettings.From, _notificationSettings.Password);
                smtp.Send(email);
                smtp.Disconnect(true);
                return await Task.FromResult(true);
            }
            catch (Exception)
            {
                return await Task.FromResult(false);
            }
        }
    }
}
