using Microsoft.AspNetCore.Mvc;
using SafeWalk.Api.Models.Requests;
using SafeWalk.Api.Models.Responses;
using SafeWalk.Application.Interfaces.Services;

namespace SafeWalk.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlertsController : ControllerBase
    {
        private readonly IAlertService _alertService;
        private readonly IJourneyService _journeyService;

        public AlertsController(
            IAlertService alertService,
            IJourneyService journeyService)
        {
            _alertService = alertService;
            _journeyService = journeyService;
        }

        // Manual panic alert trigger
        [HttpPost("panic")]
        public async Task<IActionResult> Panic(
            [FromBody] PanicAlertRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _alertService.SendJourneyAlertAsync(
                request.JourneyId,
                cancellationToken);

            if (!result.Success)
            {
                return BadRequest(new ApiErrorResponse
                {
                    Message = result.Error ?? "Failed to send panic alert."
                });
            }

            return Ok(new { success = true });
        }

        // Overdue check – can be called by a background job / scheduler or manually for now
        [HttpPost("check-overdue/{journeyId:guid}")]
        public async Task<IActionResult> CheckOverdue(Guid journeyId, CancellationToken cancellationToken)
        {
            var result = await _journeyService.EscalateIfOverdueAsync(journeyId, cancellationToken);

            if (!result.Success)
            {
                return BadRequest(new ApiErrorResponse
                {
                    Message = result.Error ?? "Failed to check overdue journey."
                });
            }

            return Ok(new { escalated = result.Value });
        }
    }
}
