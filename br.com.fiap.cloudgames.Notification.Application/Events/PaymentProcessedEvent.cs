using System;
using System.Collections.Generic;
using System.Text;

namespace br.com.fiap.cloudgames.Notification.Application.Events
{
    public class PaymentProcessedEvent
    {
        public Guid EventId { get; init; }
        public Guid UserId { get; init; }
        public string Name { get; init; } = null!;
        public string Email { get; init; } = null!;
    }
}
