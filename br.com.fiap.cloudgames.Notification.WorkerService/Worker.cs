using br.com.fiap.cloudgames.Notification.Infrastructure.Messagging;
using br.com.fiap.cloudgames.Notification.Infrastructure.Messagging.Consumers;

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
            try
            {
                _logger.LogInformation("Starting Worker");

                await _userCreatedConsumer.ConsumeAsync();

                await Task.Delay(Timeout.Infinite, stoppingToken);
                _logger.LogInformation("Stopping Worker");
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message);
            }
            finally
            {
                await _userCreatedConsumer.DisposeAsync();
            }
        }
    }
}
