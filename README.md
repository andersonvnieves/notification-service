# Notification Service

Worker de notificações. Consome os eventos `user.created` e `payment.processed` no RabbitMQ e, na implementação atual, registra a notificação no console por meio de `ConsoleEmailService`.

## Componentes

| Projeto | Responsabilidade |
| --- | --- |
| `br.com.fiap.cloudgames.Notification.Application` | Eventos, handlers e contrato de envio de e-mail. |
| `br.com.fiap.cloudgames.Notification.Infrastructure` | Conexão e consumidores RabbitMQ; implementação de e-mail no console. |
| `br.com.fiap.cloudgames.Notification.WorkerService` | Processo hospedado que inicia os consumidores. |

## Pré-requisitos

- .NET SDK 10;
- RabbitMQ na porta `5672` (painel: `15672`).

Para iniciar o RabbitMQ e a plataforma completa, use o [README da orquestração](https://github.com/andersonvnieves/orchestration/blob/main/README.md).

## Configuração local

O perfil de desenvolvimento carrega `br.com.fiap.cloudgames.Notification.WorkerService/appsettings.Development.json`. Para sobrescrever a configuração:

```powershell
$env:DOTNET_ENVIRONMENT = "Development"
$env:RabbitMQ__URI = "amqp://<USUARIO>:<SENHA>@localhost:5672/"
$env:RabbitMQ__UserCreatedEvent__Exchange = "fgc"
$env:RabbitMQ__UserCreatedEvent__RoutingKey = "user.created"
$env:RabbitMQ__PaymentProcessedEvent__Exchange = "fgc"
$env:RabbitMQ__PaymentProcessedEvent__RoutingKey = "payment.processed"
```

## Executar localmente

Na pasta `notification-service`:

```powershell
dotnet restore .\br.com.fiap.cloudgames.NotificationWorker.slnx
dotnet run --project .\br.com.fiap.cloudgames.Notification.WorkerService\br.com.fiap.cloudgames.Notification.WorkerService.csproj --launch-profile br.com.fiap.cloudgames.Notification.WorkerService
```

O worker não expõe endpoints HTTP. Confirme o processamento das mensagens pelos logs do console.

## Contêiner

```powershell
docker build -t fgc-notification-service:latest .
docker run --rm fgc-notification-service:latest
```

Informe a configuração de RabbitMQ por variáveis de ambiente quando a imagem for executada fora da orquestração.
