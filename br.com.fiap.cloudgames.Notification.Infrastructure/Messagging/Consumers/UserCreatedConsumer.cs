using br.com.fiap.cloudgames.Notification.Application.Events;
using br.com.fiap.cloudgames.Notification.Application.Handlers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace br.com.fiap.cloudgames.Notification.Infrastructure.Messagging.Consumers
{
    public class UserCreatedConsumer
    {
        private readonly UserCreatedEventHandler _handler;

        public UserCreatedConsumer(UserCreatedEventHandler handler)
        {
            _handler = handler;
        }

        public async Task ConsumeAsync(string message)
        {
            var evt = JsonSerializer.Deserialize<UserCreatedEvent>(message);

            await _handler.HandleAsync(evt);
        }
    }
}
