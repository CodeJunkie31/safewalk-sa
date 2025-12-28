using Microsoft.AspNetCore.Mvc;

namespace SafeWalk.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                service = "SafeWalk.Api",
                status = "OK",
                timestampUtc = DateTime.UtcNow
            });
        }
    }
}

