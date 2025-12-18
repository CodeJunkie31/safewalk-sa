using SafeWalk.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SafeWalk.Domain.Entities
{
    public class Alert : BaseEntity
    {
        public Guid JourneyId { get; set; }
        public string Message { get; set; } = default!;
        public DateTime SentAtUtc { get; set; } = DateTime.UtcNow;
        public string? RecipientPhoneNumber { get; set; }

        public Journey Journey { get; set; } = default!;
    }
}
