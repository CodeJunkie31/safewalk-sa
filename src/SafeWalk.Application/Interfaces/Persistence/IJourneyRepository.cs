using SafeWalk.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SafeWalk.Application.Interfaces.Persistence
{
    public interface IJourneyRepository
    {
        Task<JourneyDto?> GetByIdAsync(Guid journeyId, CancellationToken cancellationToken = default);
        Task<Guid> CreateAsync(JourneyDto journey, CancellationToken cancellationToken = default);
        Task UpdateAsync(JourneyDto journey, CancellationToken cancellationToken = default);
    }
}
