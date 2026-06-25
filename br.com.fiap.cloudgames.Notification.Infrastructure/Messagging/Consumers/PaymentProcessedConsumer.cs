using br.com.fiap.cloudgames.Notification.Application.Events;
using br.com.fiap.cloudgames.Notification.Application.Handlers;
using System.Text.Json;

namespace br.com.fiap.cloudgames.Notification.Infrastructure.Messagging.Consumers
{
    public class PaymentProcessedConsumer
    {
        private readonly PaymentProcessedEventHandler _handler;

        public PaymentProcessedConsumer(PaymentProcessedEventHandler handler)
        {
            _handler = handler;
        }

        public async Task ConsumeAsync(string message)
        {
            var evt = JsonSerializer.Deserialize<PaymentProcessedEvent>(message);

            await _handler.HandleAsync(evt);
        }
    }
}
