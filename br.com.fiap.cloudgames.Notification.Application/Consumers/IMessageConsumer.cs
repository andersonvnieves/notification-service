using System;
using System.Collections.Generic;
using System.Text;

namespace br.com.fiap.cloudgames.Notification.Application.Consumers
{
    public interface IMessageConsumer : IAsyncDisposable
    {
        Task ConsumeAsync();
    }
}
