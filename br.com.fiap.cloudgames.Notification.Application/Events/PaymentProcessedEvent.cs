using br.com.fiap.cloudgames.Notification.Application.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace br.com.fiap.cloudgames.Notification.Application.Events
{
    public class PaymentProcessedEvent : IntegrationEvent
    {
        public Guid UserId { get; set; }
        public Guid OrderId { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string Name { get; init; } = null!;
        public string Email { get; init; } = null!;
    }
}
