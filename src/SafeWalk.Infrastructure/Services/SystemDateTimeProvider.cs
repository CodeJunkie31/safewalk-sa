using SafeWalk.Application.Interfaces.Infrastructure;

namespace SafeWalk.Infrastructure.Services
{
    public class SystemDateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
