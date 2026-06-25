using br.com.fiap.cloudgames.Notification.Infrastructure.Messagging;
using br.com.fiap.cloudgames.Notification.Infrastructure.Messagging.Consumers;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace br.com.fiap.cloudgames.Notification.WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly RabbitMqConnection _rabbitConnection;
        private readonly PaymentProcessedConsumer _paymentProcessedConsumer;
        private readonly UserCreatedConsumer _userCreatedConsumer;

        public Worker(ILogger<Worker> logger, RabbitMqConnection rabbitMqConnection, PaymentProcessedConsumer paymentProcessedConsumer, UserCreatedConsumer userCreatedConsumer)
        {
            _logger = logger;
            _rabbitConnection = rabbitMqConnection;
            _paymentProcessedConsumer = paymentProcessedConsumer;
            _userCreatedConsumer = userCreatedConsumer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var connection =  _rabbitConnection.Connection;

            var channel = await connection.CreateChannelAsync();

            await channel.ExchangeDeclareAsync(
                exchange: "user-events",
                type: ExchangeType.Direct,
                durable: true);

            await channel.QueueDeclareAsync(
                queue: "notification-user-created",
                durable: true,
                exclusive: false,
                autoDelete: false);

            await channel.QueueBindAsync(
                queue: "notification-user-created",
                exchange: "user-events",
                routingKey: "user.created");

            var rabbitConsumer = new AsyncEventingBasicConsumer(channel);
            rabbitConsumer.ReceivedAsync += async (sender, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    await _userCreatedConsumer.ConsumeAsync(message);

                    await channel.BasicAckAsync(
                        deliveryTag: ea.DeliveryTag,
                        multiple: false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao processar mensagem");

                    await channel.BasicNackAsync(
                        deliveryTag: ea.DeliveryTag,
                        multiple: false,
                        requeue: true);
                }
            };

            await channel.BasicConsumeAsync(
                queue: "notification-user-created",
                autoAck: false,
                consumer: rabbitConsumer);

            while (!stoppingToken.IsCancellationRequested)
            {                
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
