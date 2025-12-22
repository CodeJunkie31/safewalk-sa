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

            await using var command = new SqlCommand(sql, connection);
            command.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = alert.Id });
            command.Parameters.Add(new SqlParameter("@JourneyId", SqlDbType.UniqueIdentifier) { Value = alert.JourneyId });
            command.Parameters.Add(new SqlParameter("@Message", SqlDbType.NVarChar, 1000) { Value = alert.Message });
            command.Parameters.Add(new SqlParameter("@SentAtUtc", SqlDbType.DateTime2) { Value = alert.SentAtUtc });
            command.Parameters.Add(new SqlParameter("@CreatedAtUtc", SqlDbType.DateTime2) { Value = DateTime.UtcNow });

            await command.ExecuteNonQueryAsync(cancellationToken);

            return alert.Id;
        }
    }
}

