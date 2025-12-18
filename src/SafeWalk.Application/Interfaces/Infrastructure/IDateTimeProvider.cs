using System;
using System.Collections.Generic;
using System.Text;

namespace SafeWalk.Application.Interfaces.Infrastructure
{
    public interface IDateTimeProvider
    {
        DateTime UtcNow { get; }
    }
}
