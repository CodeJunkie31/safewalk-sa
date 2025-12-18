using System.Security.Cryptography;
using System.Text;
using SafeWalk.Application.Common;
using SafeWalk.Application.DTOs;
using SafeWalk.Application.Interfaces.Infrastructure;
using SafeWalk.Application.Interfaces.Persistence;
using SafeWalk.Application.Interfaces.Services;

namespace SafeWalk.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IDateTimeProvider _dateTimeProvider;

        public UserService(
            IUserRepository userRepository,
            IDateTimeProvider dateTimeProvider)
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
            {
                return Result<UserDto>.Fail("A user with this email already exists.");
            }

            var passwordHash = HashPassword(password);

            var userDto = new UserDto
            {
                Id = Guid.NewGuid(),
                FullName = fullName,
                Email = email,
                PhoneNumber = phoneNumber
            };

            await _userRepository.CreateAsync(userDto, passwordHash, cancellationToken);

            return Result<UserDto>.Ok(userDto);
        }

        private static string HashPassword(string password)
        {
            // Very basic example – in real life use a proper password hasher (e.g. PBKDF2, BCrypt)
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}

