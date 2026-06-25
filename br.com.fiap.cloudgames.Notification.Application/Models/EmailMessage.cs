using System;
using System.Collections.Generic;
using System.Text;

namespace br.com.fiap.cloudgames.Notification.Application.Models
{
    public class EmailMessage
    {
        public string From { get; init; } = string.Empty;
        public string To { get; init; } = string.Empty;
        public string Subject { get; init; } = string.Empty;
        public string Body { get; init; } = string.Empty;

        public override string ToString()
        {
            return $"""
            Email Message
            -------------
            De: {From}
            Para: {To}
            Assunto: {Subject}
            
            Corpo:
            {Body}
            """;
        }
    }
}
