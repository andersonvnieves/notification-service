using br.com.fiap.cloudgames.Notification.Application.Consumers;
using br.com.fiap.cloudgames.Notification.Application.Events;
using br.com.fiap.cloudgames.Notification.Application.Handlers;
using br.com.fiap.cloudgames.Notification.Infrastructure.Config;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace br.com.fiap.cloudgames.Notification.Infrastructure.Messagging.Consumers
{
    public class PaymentProcessedEventConsumer : RabbitMqMessageConsumer<PaymentProcessedEvent>, IPaymentProcessedEventConsumer
    {
        private readonly PaymentProcessedEventHandler _handler;

        public PaymentProcessedEventConsumer(ILogger<PaymentProcessedEvent> logger,
            RabbitMqConnection rabbitMqConnection,
            PaymentProcessedEventHandler handler,
            IOptions<RabbitMqSettings> options)
            : base(rabbitMqConnection, logger, options.Value.PaymentProcessedEvent.Exchange, options.Value.PaymentProcessedEvent.RoutingKey)
        {
            _handler = handler;
        }

        public async Task ConsumeAsync()
        {
            await base.ConsumeAsync();
        }

        protected override async Task HandleMessageAsync(PaymentProcessedEvent message)
        {
            await _handler.HandleAsync(message);
        }
    }
}
