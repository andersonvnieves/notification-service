using br.com.fiap.cloudgames.Notification.Application.Consumers;

namespace br.com.fiap.cloudgames.Notification.WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IPaymentProcessedEventConsumer _paymentProcessedConsumer;
        private readonly IUserCreatedEventConsumer _userCreatedConsumer;

        public Worker(ILogger<Worker> logger,
            IPaymentProcessedEventConsumer paymentProcessedConsumer,
            IUserCreatedEventConsumer userCreatedConsumer)
        {
            _logger = logger;
            _paymentProcessedConsumer = paymentProcessedConsumer;
            _userCreatedConsumer = userCreatedConsumer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _logger.LogInformation("Starting Worker");

                await Task.WhenAll(
                _paymentProcessedConsumer.ConsumeAsync(),
                _userCreatedConsumer.ConsumeAsync());


                _logger.LogInformation("Worker Started");
                await Task.Delay(Timeout.Infinite, stoppingToken);
                _logger.LogInformation("Stopping Worker");
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message);
            }
            finally
            {
                await _paymentProcessedConsumer.DisposeAsync();
                await _userCreatedConsumer.DisposeAsync();
                _logger.LogInformation("Worker Finished");
            }
        }
    }
}
