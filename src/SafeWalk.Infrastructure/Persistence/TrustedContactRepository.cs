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

        public async Task<Guid> CreateAsync(TrustedContactDto contact, CancellationToken cancellationToken = default)
        {
            const string sql = @"
INSERT INTO TrustedContacts (Id, UserId, ContactName, ContactPhoneNumber, Relationship, CreatedAtUtc)
VALUES (@Id, @UserId, @ContactName, @ContactPhoneNumber, @Relationship, @CreatedAtUtc);";

            await using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync(cancellationToken);

            await using var cmd = new SqlCommand(sql, connection);
            cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = contact.Id });
            cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.UniqueIdentifier) { Value = contact.UserId });
            cmd.Parameters.Add(new SqlParameter("@ContactName", SqlDbType.NVarChar, 200) { Value = contact.ContactName });
            cmd.Parameters.Add(new SqlParameter("@ContactPhoneNumber", SqlDbType.NVarChar, 30) { Value = contact.ContactPhoneNumber });
            cmd.Parameters.Add(new SqlParameter("@Relationship", SqlDbType.NVarChar, 100) { Value = contact.Relationship });
            cmd.Parameters.Add(new SqlParameter("@CreatedAtUtc", SqlDbType.DateTime2) { Value = contact.CreatedAtUtc });

            await cmd.ExecuteNonQueryAsync(cancellationToken);
            return contact.Id;
        }

        public async Task<IReadOnlyList<TrustedContactDto>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            const string sql = @"
SELECT Id, UserId, ContactName, ContactPhoneNumber, Relationship, CreatedAtUtc
FROM TrustedContacts
WHERE UserId = @UserId
ORDER BY CreatedAtUtc DESC;";

            var results = new List<TrustedContactDto>();

            await using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync(cancellationToken);

            await using var cmd = new SqlCommand(sql, connection);
            cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.UniqueIdentifier) { Value = userId });

            await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
            while (await reader.ReadAsync(cancellationToken))
            {
                results.Add(new TrustedContactDto
                {
                    Id = reader.GetGuid(reader.GetOrdinal("Id")),
                    UserId = reader.GetGuid(reader.GetOrdinal("UserId")),
                    ContactName = reader.GetString(reader.GetOrdinal("ContactName")),
                    ContactPhoneNumber = reader.GetString(reader.GetOrdinal("ContactPhoneNumber")),
                    Relationship = reader.GetString(reader.GetOrdinal("Relationship")),
                    CreatedAtUtc = reader.GetDateTime(reader.GetOrdinal("CreatedAtUtc"))
                });
            }

            return results;
        }
    }
}
