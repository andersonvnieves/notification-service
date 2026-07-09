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
        private readonly ILogger<PaymentProcessedEvent> _logger;

        public PaymentProcessedEventConsumer(ILogger<PaymentProcessedEvent> logger,
            RabbitMqConnection rabbitMqConnection,
            PaymentProcessedEventHandler handler,
            IOptions<RabbitMqSettings> options)
            : base(rabbitMqConnection, logger, options.Value.PaymentProcessedEvent.Exchange, options.Value.PaymentProcessedEvent.RoutingKey)
        {
            _handler = handler;
            _logger = logger;
        }

        public new async Task ConsumeAsync()
        {
            await base.ConsumeAsync();
        }

        protected override async Task HandleMessageAsync(PaymentProcessedEvent message)
        {
            using var scope = _logger.BeginScope(new Dictionary<string, object?>
            {
                ["CorrelationId"] = message.CorrelationId,
                ["EventId"] = message.EventId
            });

            _logger.LogInformation(
                "Consuming PaymentProcessedEvent. EventId={EventId}, CorrelationId={CorrelationId}",
                message.EventId,
                message.CorrelationId);

            await _handler.HandleAsync(message);

            _logger.LogInformation(
                "PaymentProcessedEvent processed successfully. EventId={EventId}, CorrelationId={CorrelationId}",
                message.EventId,
                message.CorrelationId);
        }
    }
}
