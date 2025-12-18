using SafeWalk.Application.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SafeWalk.Application.Interfaces.Services
{
    public interface IAlertService
    {
        Task<Result<bool>> SendJourneyAlertAsync(
            Guid journeyId,
            CancellationToken cancellationToken = default);
    }
}
