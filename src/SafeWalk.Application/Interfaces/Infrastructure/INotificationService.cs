using System;
using System.Collections.Generic;
using System.Text;

namespace SafeWalk.Application.Interfaces.Infrastructure
{
    public interface INotificationService
    {
        Task SendSmsAsync(
            string phoneNumber,
            string message,
            CancellationToken cancellationToken = default);
    }
}
