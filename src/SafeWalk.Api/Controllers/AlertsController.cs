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

        public AlertsController(IAlertService alertService)
        {
            _alertService = alertService;
        }

        [HttpPost("panic")]
        public async Task<IActionResult> Panic([FromBody] PanicRequest req, CancellationToken ct)
        {
            var result = await _alertService.PanicAsync(req.JourneyId, ct);
            if (!result.Success) return BadRequest(new { error = result.Error });
            return Ok(new { success = result.Value });
        }

        [HttpPost("check-overdue/{journeyId:guid}")]
        public async Task<IActionResult> CheckOverdue(Guid journeyId, CancellationToken ct)
        {
            var result = await _alertService.CheckOverdueAsync(journeyId, ct);
            if (!result.Success) return BadRequest(new { error = result.Error });
            return Ok(new { escalated = result.Value });
        }
    }

    public class PanicRequest
    {
        public Guid JourneyId { get; set; }
    }


}
