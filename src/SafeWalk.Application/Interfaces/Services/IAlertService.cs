using SafeWalk.Application.Common;

namespace SafeWalk.Application.Interfaces.Services
{
    public interface IAlertService
    {
        Task<Result<bool>> PanicAsync(Guid journeyId, CancellationToken cancellationToken = default);
        Task<Result<bool>> CheckOverdueAsync(Guid journeyId, CancellationToken cancellationToken = default);
    }
}
