using SafeWalk.Application.Common;
using SafeWalk.Application.DTOs;

namespace SafeWalk.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<Result<UserDto>> RegisterAsync(
            string fullName,
            string email,
            string phoneNumber,
            string password,
            CancellationToken cancellationToken = default);

        Task<UserDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}


