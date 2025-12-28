using Microsoft.Data.SqlClient;
using SafeWalk.Application.DTOs;
using SafeWalk.Application.Interfaces.Persistence;
using System.Data;

namespace SafeWalk.Infrastructure.Persistence
{
    public class AlertRepository : IAlertRepository
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public AlertRepository(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Guid> CreateAsync(AlertDto alert, CancellationToken cancellationToken = default)
        {
            const string sql = @"
INSERT INTO Alerts (Id, JourneyId, Message, SentAtUtc, CreatedAtUtc)
VALUES (@Id, @JourneyId, @Message, @SentAtUtc, @CreatedAtUtc);";

            await using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync(cancellationToken);

            await using var cmd = new SqlCommand(sql, connection);
            cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = alert.Id });
            cmd.Parameters.Add(new SqlParameter("@JourneyId", SqlDbType.UniqueIdentifier) { Value = alert.JourneyId });
            cmd.Parameters.Add(new SqlParameter("@Message", SqlDbType.NVarChar, 1000) { Value = alert.Message });
            cmd.Parameters.Add(new SqlParameter("@SentAtUtc", SqlDbType.DateTime2) { Value = alert.SentAtUtc });
            cmd.Parameters.Add(new SqlParameter("@CreatedAtUtc", SqlDbType.DateTime2) { Value = alert.CreatedAtUtc });

            await cmd.ExecuteNonQueryAsync(cancellationToken);
            return alert.Id;
        }

        public async Task<IReadOnlyList<AlertDto>> GetByJourneyIdAsync(Guid journeyId, CancellationToken cancellationToken = default)
        {
            const string sql = @"
SELECT Id, JourneyId, Message, SentAtUtc, CreatedAtUtc
FROM Alerts
WHERE JourneyId = @JourneyId
ORDER BY CreatedAtUtc DESC;";

            var results = new List<AlertDto>();

            await using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync(cancellationToken);

            await using var cmd = new SqlCommand(sql, connection);
            cmd.Parameters.Add(new SqlParameter("@JourneyId", SqlDbType.UniqueIdentifier) { Value = journeyId });

            await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
            while (await reader.ReadAsync(cancellationToken))
            {
                results.Add(new AlertDto
                {
                    Id = reader.GetGuid(reader.GetOrdinal("Id")),
                    JourneyId = reader.GetGuid(reader.GetOrdinal("JourneyId")),
                    Message = reader.GetString(reader.GetOrdinal("Message")),
                    SentAtUtc = reader.GetDateTime(reader.GetOrdinal("SentAtUtc")),
                    CreatedAtUtc = reader.GetDateTime(reader.GetOrdinal("CreatedAtUtc")),
                });
            }

            return results;
        }
    }
}

