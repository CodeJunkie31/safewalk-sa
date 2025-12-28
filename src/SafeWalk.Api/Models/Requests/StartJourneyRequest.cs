namespace SafeWalk.Api.Models.Requests
{
    public class StartJourneyRequest
    {
        public Guid UserId { get; set; }
        public string StartLocation { get; set; } = default!;
        public string Destination { get; set; } = default!;
        public DateTime ExpectedArrivalTimeUtc { get; set; }
    }
}

