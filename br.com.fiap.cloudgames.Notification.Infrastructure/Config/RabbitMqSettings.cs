using System;
using System.Collections.Generic;
using System.Text;

namespace br.com.fiap.cloudgames.Notification.Infrastructure.Config
{
    public class RabbitMqSettings
    {
        public string URI { get; set; }
        public required RabbitMqQueueDetailsSettings UserCreatedEvent { get; set; }
        public required RabbitMqQueueDetailsSettings PaymentProcessedEvent { get; set; }
    }
}
