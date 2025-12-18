using SafeWalk.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SafeWalk.Application.Interfaces.Persistence
{
    public interface IAlertRepository
    {
        Task<Guid> CreateAsync(AlertDto alert, CancellationToken cancellationToken = default);
    }
}
