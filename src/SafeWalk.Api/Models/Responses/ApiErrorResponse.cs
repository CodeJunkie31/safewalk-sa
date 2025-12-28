namespace SafeWalk.Api.Models.Responses
{
    public class ApiErrorResponse
    {
        public string Message { get; set; } = default!;
        public string? Details { get; set; }
    }
}
