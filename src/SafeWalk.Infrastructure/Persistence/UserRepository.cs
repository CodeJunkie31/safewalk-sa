using Microsoft.Data.SqlClient;
using SafeWalk.Application.DTOs;
using SafeWalk.Application.Interfaces.Persistence;
using System.Data;

namespace SafeWalk.Infrastructure.Persistence
{
    public class UserRepository : IUserRepository
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public UserRepository(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<UserDto?> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            const string sql = @"
                SELECT Id, FullName, Email, PhoneNumber
                FROM Users
                WHERE Id = @Id";

            await using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync(cancellationToken);

            await using var command = new SqlCommand(sql, connection);
            command.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = userId });

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);
            if (!await reader.ReadAsync(cancellationToken))
            {
                return null;
            }

            return new UserDto
            {
                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                FullName = reader.GetString(reader.GetOrdinal("FullName")),
                Email = reader.GetString(reader.GetOrdinal("Email")),
                PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber"))
            };
        }

        public async Task<UserDto?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            const string sql = @"
                SELECT Id, FullName, Email, PhoneNumber
                FROM Users
                WHERE Email = @Email";

            await using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync(cancellationToken);

            await using var command = new SqlCommand(sql, connection);
            command.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 200) { Value = email });

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);
            if (!await reader.ReadAsync(cancellationToken))
            {
                return null;
            }

            return new UserDto
            {
                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                FullName = reader.GetString(reader.GetOrdinal("FullName")),
                Email = reader.GetString(reader.GetOrdinal("Email")),
                PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber"))
            };
        }

        public async Task<Guid> CreateAsync(
            UserDto user,
            string passwordHash,
            CancellationToken cancellationToken = default)
        {
            const string sql = @"
                INSERT INTO Users (Id, FullName, Email, PhoneNumber, PasswordHash, CreatedAtUtc)
                VALUES (@Id, @FullName, @Email, @PhoneNumber, @PasswordHash, @CreatedAtUtc);";

            await using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync(cancellationToken);

            await using var command = new SqlCommand(sql, connection);
            command.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = user.Id });
            command.Parameters.Add(new SqlParameter("@FullName", SqlDbType.NVarChar, 200) { Value = user.FullName });
            command.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 200) { Value = user.Email });
            command.Parameters.Add(new SqlParameter("@PhoneNumber", SqlDbType.NVarChar, 30) { Value = user.PhoneNumber });
            command.Parameters.Add(new SqlParameter("@PasswordHash", SqlDbType.NVarChar, -1) { Value = passwordHash });
            command.Parameters.Add(new SqlParameter("@CreatedAtUtc", SqlDbType.DateTime2) { Value = DateTime.UtcNow });

            await command.ExecuteNonQueryAsync(cancellationToken);

            return user.Id;
        }
    }
}
