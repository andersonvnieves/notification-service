using br.com.fiap.cloudgames.Notification.Application.Events;
using br.com.fiap.cloudgames.Notification.Application.Models;
using br.com.fiap.cloudgames.Notification.Application.Services;

namespace br.com.fiap.cloudgames.Notification.Application.Handlers
{
    public class UserCreatedEventHandler
    {
        private readonly IEmailService _emailService;

        public UserCreatedEventHandler(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task HandleAsync(UserCreatedEvent userCreatedEvent)
        {
            var msg = new EmailMessage()
            {
                From = "noreply@fgc.com.br",
                To = userCreatedEvent.Email,
                Subject = "Bora Jogar!",
                Body = $"Bem vindo, {userCreatedEvent.Name}"
            };

            await _emailService.SendEmailAsync(msg);
        } 
    }
}
