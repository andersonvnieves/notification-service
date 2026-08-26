using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using br.com.fiap.cloudgames.Notification.Application.Events;
using br.com.fiap.cloudgames.Notification.Application.Handlers;
using br.com.fiap.cloudgames.Notification.Infrastructure.Email;
using Microsoft.Extensions.Logging;
using System.Text.Json;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace br.com.fiap.cloudgames.Notification.Lambda
{
    public class Function
    {
        private readonly ILogger<ConsoleEmailService> _logger;
        private readonly ConsoleEmailService _emailService;

        public Function()
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole(); // Adiciona o provedor de console que a Lambda lê
                builder.SetMinimumLevel(LogLevel.Information);
            });

            // Cria a instância do ILogger<ConsoleEmailService> exigida pelo seu serviço
            _logger = loggerFactory.CreateLogger<ConsoleEmailService>();

            // Instancia o seu serviço passando o logger corretamente
            _emailService = new ConsoleEmailService(_logger);
        }


        /// <summary>
        /// This method is called for every Lambda invocation. This method takes in an SQS event object and can be used 
        /// to respond to SQS messages.
        /// </summary>
        /// <param name="evnt">The event for the Lambda function handler to process.</param>
        /// <param name="context">The ILambdaContext that provides methods for logging and describing the Lambda environment.</param>
        /// <returns></returns>
        public async Task<SQSBatchResponse> FunctionHandler(SQSEvent evnt, ILambdaContext context)
        {
            foreach (var message in evnt.Records)
            {
                context.Logger.LogInformation($"Mensagem recebida da fila (ARN): {message.EventSourceArn}");
                context.Logger.LogInformation($"Conteúdo da mensagem: {message.Body}");

                // Descobre de qual fila a mensagem veio olhando o ARN ou o nome
                if (message.EventSourceArn.Contains("user-created-queue"))
                {
                    var evento = JsonSerializer.Deserialize<UserCreatedEvent>(message.Body);
                    var handler = new UserCreatedEventHandler(new ConsoleEmailService(context.Logger));
                    await handler.HandleAsync(evento);
                }
                else if (message.EventSourceArn.Contains("payment-processed-queue"))
                {
                    var evento = JsonSerializer.Deserialize<PaymentProcessedEvent>(message.Body);
                    var handler = new PaymentProcessedEventHandler(new ConsoleEmailService(context.Logger));
                    await handler.HandleAsync(evento);
                }
            }

            return new SQSBatchResponse();
        }
        
    }
}