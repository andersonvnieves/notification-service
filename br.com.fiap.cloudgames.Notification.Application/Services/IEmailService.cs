using br.com.fiap.cloudgames.Notification.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace br.com.fiap.cloudgames.Notification.Application.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailMessage emailMessage);
    }
}
