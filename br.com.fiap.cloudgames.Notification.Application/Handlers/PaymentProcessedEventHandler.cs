using br.com.fiap.cloudgames.Notification.Application.Events;
using br.com.fiap.cloudgames.Notification.Application.Interfaces;
using br.com.fiap.cloudgames.Notification.Application.Models;

namespace br.com.fiap.cloudgames.Notification.Application.Handlers
{
    public class PaymentProcessedEventHandler
    {
        private readonly IEmailService _emailService;

        public PaymentProcessedEventHandler(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task HandleAsync(PaymentProcessedEvent paymentProcessedEvent)
        {
            var msg = new EmailMessage()
            {
                From = "noreply@fgc.com.br",
                To = paymentProcessedEvent.Email,
                Subject = "Pagamento aprovado!",
                Body = $"Olá, {paymentProcessedEvent.Name}"
            };

            await _emailService.SendEmailAsync(msg);
        }
    }
}
