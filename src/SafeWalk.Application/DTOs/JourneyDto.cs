using SafeWalk.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SafeWalk.Application.DTOs
{
    public class JourneyDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string StartLocation { get; set; } = default!;
        public string Destination { get; set; } = default!;
        public DateTime StartTimeUtc { get; set; }
        public DateTime ExpectedArrivalTimeUtc { get; set; }
        public DateTime? ActualArrivalTimeUtc { get; set; }
        public string Status { get; set; } = default!; 
    }
}
