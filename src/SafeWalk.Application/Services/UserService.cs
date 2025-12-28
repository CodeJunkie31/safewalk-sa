using System.Security.Cryptography;
using System.Text;
using SafeWalk.Application.Common;
using SafeWalk.Application.DTOs;
using SafeWalk.Application.Interfaces.Infrastructure;
using SafeWalk.Application.Interfaces.Persistence;
using SafeWalk.Application.Interfaces.Services;
using SafeWalk.Domain.Entities;

namespace SafeWalk.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IDateTimeProvider _dateTimeProvider;

        public UserService(IUserRepository userRepository, IDateTimeProvider dateTimeProvider)
        {
            _userRepository = userRepository;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Result<UserDto>> RegisterAsync(
            string fullName,
            string email,
            string phoneNumber,
            string password,
            CancellationToken cancellationToken = default)
        {
            var existing = await _userRepository.GetByEmailAsync(email, cancellationToken);
            if (existing != null)
                return Result<UserDto>.Fail("A user with this email already exists.");

            var now = _dateTimeProvider.UtcNow;
            var id = Guid.NewGuid();

            var user = new User
            {
                Id = id,
                FullName = fullName,
                Email = email,
                PhoneNumber = phoneNumber,
                PasswordHash = HashPassword(password),
                CreatedAtUtc = now,
                UpdatedAtUtc = now
            };

            await _userRepository.CreateAsync(user, cancellationToken);

            // Return DTO back to API
            var dto = new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };

            return Result<UserDto>.Ok(dto);
        }

        public async Task<UserDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByIdAsync(id, cancellationToken);
            if (user == null) return null;

            return new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
        }
       

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}

