using br.com.fiap.cloudgames.Notification.Application.Handlers;
using br.com.fiap.cloudgames.Notification.Application.Services;
using br.com.fiap.cloudgames.Notification.Infrastructure.Config;
using br.com.fiap.cloudgames.Notification.Infrastructure.Email;
using br.com.fiap.cloudgames.Notification.Infrastructure.Messagging;
using br.com.fiap.cloudgames.Notification.Infrastructure.Messagging.Consumers;
using br.com.fiap.cloudgames.Notification.WorkerService;

var builder = Host.CreateApplicationBuilder(args);

//Settings
builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMQ"));

builder.Services.AddSingleton<RabbitMqConnection>();
builder.Services.AddSingleton<IEmailService, ConsoleEmailService>();

builder.Services.AddSingleton<PaymentProcessedConsumer>();
builder.Services.AddSingleton<UserCreatedConsumer>();

builder.Services.AddSingleton<PaymentProcessedEventHandler>();
builder.Services.AddSingleton<UserCreatedEventHandler>();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
