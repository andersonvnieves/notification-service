using br.com.fiap.cloudgames.Notification.Application.Models;
using br.com.fiap.cloudgames.Notification.Application.Services;
using Microsoft.Extensions.Logging;

namespace br.com.fiap.cloudgames.Notification.Infrastructure.Email
{
    public class ConsoleEmailService : IEmailService
    {
        private readonly ILogger<ConsoleEmailService> _logger;

        public ConsoleEmailService(ILogger<ConsoleEmailService> logger)
        {
            _logger = logger;
        }

        public async Task SendEmailAsync(EmailMessage emailMessage) => _logger.LogInformation(emailMessage.ToString());
    }
}
