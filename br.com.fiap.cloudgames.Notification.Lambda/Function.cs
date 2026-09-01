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
        public Function() { }


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
                var queueName = message.EventSourceArn.Split(':').Last();

                switch (queueName)
                {
                    case string name when name.Contains("user-created-queue"):
                        var userEvent = JsonSerializer.Deserialize<UserCreatedEvent>(message.Body);
                        var userHandler = new UserCreatedEventHandler(new ConsoleEmailService(context.Logger));
                        await userHandler.HandleAsync(userEvent);
                        break;

                    case string name when name.Contains("payment-processed-queue"):
                        var paymentEvent = JsonSerializer.Deserialize<PaymentProcessedEvent>(message.Body);
                        var paymentHandler = new PaymentProcessedEventHandler(new ConsoleEmailService(context.Logger));
                        await paymentHandler.HandleAsync(paymentEvent);
                        break;

                    default:
                        context.Logger.LogWarning("Event type not found for the queue with ARN: {Arn}", message.EventSourceArn);
                        break;
                }
            }

            return new SQSBatchResponse();
        }
        
    }
}