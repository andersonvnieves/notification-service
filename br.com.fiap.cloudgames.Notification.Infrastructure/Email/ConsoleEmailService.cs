using br.com.fiap.cloudgames.Notification.Application.Interfaces;
using br.com.fiap.cloudgames.Notification.Application.Models;

namespace br.com.fiap.cloudgames.Notification.Infrastructure.Email
{
    public class ConsoleEmailService : IEmailService
    {
        public async Task SendEmailAsync(EmailMessage emailMessage) => Console.WriteLine(emailMessage.ToString());
    }
}
