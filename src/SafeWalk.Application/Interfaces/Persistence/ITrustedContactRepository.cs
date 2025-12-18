using SafeWalk.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SafeWalk.Application.Interfaces.Persistence
{
    public interface ITrustedContactRepository
    {
        Task<IReadOnlyList<TrustedContactDto>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
