using System;
using System.Collections.Generic;
using System.Text;

namespace SafeWalk.Domain.Interfaces
{
    public interface ISmsNotificationService
    {
        Task SendSmsAsync(string phoneNumber, string message, CancellationToken cancellationToken = default);
    }
}
