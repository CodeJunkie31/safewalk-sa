using SafeWalk.Application.Common;
using SafeWalk.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
