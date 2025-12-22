using SafeWalk.Application.Interfaces.Infrastructure;

namespace SafeWalk.Infrastructure.Services
{
    // Later you can plug in Twilio / Clickatell here.
    public class SmsNotificationService : INotificationService
    {
        public Task SendSmsAsync(
            string phoneNumber,
            string message,
            CancellationToken cancellationToken = default)
        {
            // TEMP: log to console for local dev
            Console.WriteLine($"[SafeWalk SMS] To: {phoneNumber} | Message: {message}");
            return Task.CompletedTask; // to send sms', integrate with an SMS provider here
        }
    }
}

