using System;
using System.Collections.Generic;
using System.Text;

namespace SafeWalk.Domain.Interfaces
{
    public interface IDateTimeProvider
    {
        DateTime UtcNow { get; }
    }
}
