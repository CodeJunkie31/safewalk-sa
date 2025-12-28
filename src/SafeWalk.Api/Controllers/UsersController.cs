using Microsoft.AspNetCore.Mvc;
using SafeWalk.Api.Models.Requests;
using SafeWalk.Api.Models.Responses;
using SafeWalk.Application.Interfaces.Services;

namespace SafeWalk.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromBody] RegisterUserRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _userService.RegisterAsync(
                request.FullName,
                request.Email,
                request.PhoneNumber,
                request.Password,
                cancellationToken);

            if (!result.Success || result.Value == null)
            {
                return BadRequest(new
                {
                    error = result.Error ?? "Registration failed."
                });
            }

            var user = result.Value;

            var response = new UserResponse
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };

            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var user = await _userService.GetByIdAsync(id, cancellationToken);

            if (user == null)
                return NotFound(new { message = "User not found." });

            var response = new UserResponse
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };

            return Ok(response);
        }

    }
}
