using SafeWalk.Domain.Common;
using SafeWalk.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SafeWalk.Domain.Entities
{
    public class Journey : BaseEntity
    {
        public Guid UserId { get; set; }
        public string StartLocation { get; set; } = default!;
        public string Destination { get; set; } = default!;
        public DateTime StartTimeUtc { get; set; }
        public DateTime ExpectedArrivalTimeUtc { get; set; }
        public DateTime? ActualArrivalTimeUtc { get; set; }
        public JourneyStatus Status { get; set; } = JourneyStatus.Pending;
        public bool AlertSent { get; set; }

        public User User { get; set; } = default!;
        public ICollection<Alert> Alerts { get; set; } = new List<Alert>();
    }
}
