namespace SafeWalk.Api.Models.Responses
{
    public class JourneyResponse
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

