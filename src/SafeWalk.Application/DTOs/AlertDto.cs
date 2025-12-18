using System;
using System.Collections.Generic;
using System.Text;

namespace SafeWalk.Application.DTOs
{
    public class AlertDto
    {
        public Guid Id { get; set; }
        public Guid JourneyId { get; set; }
        public string Message { get; set; } = default!;
        public DateTime SentAtUtc { get; set; }
    }
}
