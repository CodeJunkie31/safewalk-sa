using Microsoft.AspNetCore.Mvc;
using SafeWalk.Application.DTOs;
using SafeWalk.Application.Interfaces.Persistence;

namespace SafeWalk.Api.Controllers
{
     [ApiController]
    [Route("api/[controller]")]

    public class TrustedContactsController : ControllerBase
    {
       
        
            private readonly ITrustedContactRepository _repo;

            public TrustedContactsController(ITrustedContactRepository repo)
            {
                _repo = repo;
            }

            [HttpPost]
            public async Task<IActionResult> Add([FromBody] AddTrustedContactRequest req, CancellationToken ct)
            {
                var dto = new TrustedContactDto
                {
                    Id = Guid.NewGuid(),
                    UserId = req.UserId,
                    ContactName = req.ContactName,
                    ContactPhoneNumber = req.ContactPhoneNumber,
                    Relationship = req.Relationship,
                    CreatedAtUtc = DateTime.UtcNow
                };

                await _repo.CreateAsync(dto, ct);
                return Ok(dto);
            }

            [HttpGet("user/{userId:guid}")]
            public async Task<IActionResult> GetForUser(Guid userId, CancellationToken ct)
            {
                var contacts = await _repo.GetByUserIdAsync(userId, ct);
                return Ok(contacts);
            }
        }

        public class AddTrustedContactRequest
        {
            public Guid UserId { get; set; }
            public string ContactName { get; set; } = "";
            public string ContactPhoneNumber { get; set; } = "";
            public string Relationship { get; set; } = "";
        }

    }

