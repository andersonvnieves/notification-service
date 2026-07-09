using br.com.fiap.cloudgames.Notification.Application.Consumers;
using br.com.fiap.cloudgames.Notification.Application.Events;
using br.com.fiap.cloudgames.Notification.Application.Handlers;
using br.com.fiap.cloudgames.Notification.Infrastructure.Config;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace br.com.fiap.cloudgames.Notification.Infrastructure.Messagging.Consumers
{
    public class UserCreatedEventConsumer : RabbitMqMessageConsumer<UserCreatedEvent>, IUserCreatedEventConsumer
    {
        private readonly UserCreatedEventHandler _handler;

        private IChannel _channel;

        public UserCreatedEventConsumer(ILogger<UserCreatedEvent> logger,
            RabbitMqConnection rabbitMqConnection,
            UserCreatedEventHandler handler,
            IOptions<RabbitMqSettings> options)
            : base(rabbitMqConnection, logger, options.Value.UserCreatedEvent.Exchange, options.Value.UserCreatedEvent.RoutingKey)
        {
            _handler = handler;
        }

        public async Task ConsumeAsync()
        {
            await base.ConsumeAsync();
        }

        protected override async Task HandleMessageAsync(UserCreatedEvent message)
        {
            await _handler.HandleAsync(message);
        }
    }
}
