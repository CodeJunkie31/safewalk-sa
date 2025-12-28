using Microsoft.AspNetCore.Mvc;
using SafeWalk.Api.Models.Requests;
using SafeWalk.Api.Models.Responses;
using SafeWalk.Application.Interfaces.Services;

namespace SafeWalk.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JourneysController : ControllerBase
    {
        private readonly IJourneyService _journeyService;

        public JourneysController(IJourneyService journeyService)
        {
            _journeyService = journeyService;
        }

        [HttpPost("start")]
        public async Task<IActionResult> Start(
            [FromBody] StartJourneyRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _journeyService.StartJourneyAsync(
                request.UserId,
                request.StartLocation,
                request.Destination,
                request.ExpectedArrivalTimeUtc,
                cancellationToken);

            if (!result.Success || result.Value == null)
            {
                return BadRequest(new ApiErrorResponse
                {
                    Message = result.Error ?? "Could not start journey."
                });
            }

            var dto = result.Value;

            var response = new JourneyResponse
            {
                Id = dto.Id,
                UserId = dto.UserId,
                StartLocation = dto.StartLocation,
                Destination = dto.Destination,
                StartTimeUtc = dto.StartTimeUtc,
                ExpectedArrivalTimeUtc = dto.ExpectedArrivalTimeUtc,
                ActualArrivalTimeUtc = dto.ActualArrivalTimeUtc,
                Status = dto.Status
            };

            return Ok(response);
        }

        [HttpPost("complete")]
        public async Task<IActionResult> Complete(
            [FromBody] CompleteJourneyRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _journeyService.CompleteJourneyAsync(
                request.JourneyId,
                cancellationToken);

            if (!result.Success)
            {
                return BadRequest(new ApiErrorResponse
                {
                    Message = result.Error ?? "Could not complete journey."
                });
            }

            return Ok(new { success = true });
        }
    }
}

