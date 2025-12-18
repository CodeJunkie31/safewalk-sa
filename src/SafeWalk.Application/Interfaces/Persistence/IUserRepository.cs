using SafeWalk.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SafeWalk.Application.Interfaces.Persistence
{
    public interface IUserRepository
    {
        Task<UserDto?> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<UserDto?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<Guid> CreateAsync(UserDto user, string passwordHash, CancellationToken cancellationToken = default);
    }
}
