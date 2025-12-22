using Microsoft.Data.SqlClient;
using SafeWalk.Application.DTOs;
using SafeWalk.Application.Interfaces.Persistence;
using System.Data;

namespace SafeWalk.Infrastructure.Persistence
{
    public class TrustedContactRepository : ITrustedContactRepository
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public TrustedContactRepository(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IReadOnlyList<TrustedContactDto>> GetByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            const string sql = @"
                SELECT Id, UserId, ContactName, ContactPhoneNumber, Relationship
                FROM TrustedContacts
                WHERE UserId = @UserId";

            var contacts = new List<TrustedContactDto>();

            await using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync(cancellationToken);

            await using var command = new SqlCommand(sql, connection);
            command.Parameters.Add(new SqlParameter("@UserId", SqlDbType.UniqueIdentifier) { Value = userId });

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);
            while (await reader.ReadAsync(cancellationToken))
            {
                var dto = new TrustedContactDto
                {
                    Id = reader.GetGuid(reader.GetOrdinal("Id")),
                    UserId = reader.GetGuid(reader.GetOrdinal("UserId")),
                    ContactName = reader.GetString(reader.GetOrdinal("ContactName")),
                    ContactPhoneNumber = reader.GetString(reader.GetOrdinal("ContactPhoneNumber")),
                    Relationship = reader.IsDBNull(reader.GetOrdinal("Relationship"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("Relationship"))
                };

                contacts.Add(dto);
            }

            return contacts;
        }
    }
}

