using br.com.fiap.cloudgames.Notification.Application.Events;
using br.com.fiap.cloudgames.Notification.Application.Handlers;
using br.com.fiap.cloudgames.Notification.Infrastructure.Config;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace br.com.fiap.cloudgames.Notification.Infrastructure.Messagging.Consumers
{
    public class UserCreatedConsumer : IAsyncDisposable
    {
        private readonly ILogger<UserCreatedConsumer> _logger;
        private readonly RabbitMqConnection _rabbitConnection;
        private readonly UserCreatedEventHandler _handler;
        private readonly IOptions<RabbitMqSettings> _options;

        private IChannel _channel;

        public UserCreatedConsumer(ILogger<UserCreatedConsumer> logger, RabbitMqConnection rabbitMqConnection, UserCreatedEventHandler handler, IOptions<RabbitMqSettings> options)
        {
            _logger = logger;
            _rabbitConnection = rabbitMqConnection;
            _handler = handler;
            _options = options;
        }

        public async Task ConsumeAsync()
        {
            _logger.LogInformation("Starting UserCreatedConsumer");
            var connection = _rabbitConnection.Connection;
            _logger.LogInformation($"Connection to RabbitMQ is open: {connection.IsOpen}");
            _channel = await connection.CreateChannelAsync();

            await _channel.ExchangeDeclareAsync(
                exchange: _options.Value.UserCreatedEvent.Exchange,
                type: ExchangeType.Topic,
                durable: true);

            await _channel.QueueDeclareAsync(
                queue: _options.Value.UserCreatedEvent.RoutingKey,
                durable: true,
                exclusive: false,
                autoDelete: false);

            await _channel.QueueBindAsync(
                queue: _options.Value.UserCreatedEvent.RoutingKey,
                exchange: _options.Value.UserCreatedEvent.Exchange,
                routingKey: _options.Value.UserCreatedEvent.RoutingKey);

            var rabbitConsumer = new AsyncEventingBasicConsumer(_channel);
            rabbitConsumer.ReceivedAsync += async (sender, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    var evt = JsonSerializer.Deserialize<UserCreatedEvent>(message);
                    if (evt is null)
                    {
                        throw new InvalidOperationException("Mensagem inválida.");
                    }
                    await _handler.HandleAsync(evt);

                    await _channel.BasicAckAsync(
                        deliveryTag: ea.DeliveryTag,
                        multiple: false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao processar mensagem");

                    await _channel.BasicNackAsync(
                        deliveryTag: ea.DeliveryTag,
                        multiple: false,
                        requeue: true);
                }
            };

            await _channel.BasicConsumeAsync(
                queue: _options.Value.UserCreatedEvent.RoutingKey,
                autoAck: false,
                consumer: rabbitConsumer);            
        }

        public async ValueTask DisposeAsync()
        {
            if (_channel is not null)
            {
                await _channel.CloseAsync();
                await _channel.DisposeAsync();
            }
        }
    }
}
