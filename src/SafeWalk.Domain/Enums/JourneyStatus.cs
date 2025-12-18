using System;
using System.Collections.Generic;
using System.Text;

namespace SafeWalk.Domain.Enums
{
    public enum JourneyStatus
    {
        Pending = 0,
        InProgress = 1,
        Completed = 2,
        Escalated = 3,
        Cancelled = 4
    }
}
