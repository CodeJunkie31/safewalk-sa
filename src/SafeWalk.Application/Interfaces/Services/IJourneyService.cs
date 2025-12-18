using SafeWalk.Application.Common;
using SafeWalk.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SafeWalk.Application.Interfaces.Services
{
    public interface IJourneyService
    {
        Task<Result<JourneyDto>> StartJourneyAsync(
            Guid userId,
            string startLocation,
            string destination,
            DateTime expectedArrivalUtc,
            CancellationToken cancellationToken = default);

        Task<Result<bool>> CompleteJourneyAsync(
            Guid journeyId,
            CancellationToken cancellationToken = default);

        Task<Result<bool>> EscalateIfOverdueAsync(
            Guid journeyId,
            CancellationToken cancellationToken = default);
    }
}
