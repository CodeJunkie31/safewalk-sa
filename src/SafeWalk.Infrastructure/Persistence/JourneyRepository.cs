using Microsoft.Data.SqlClient;
using SafeWalk.Application.DTOs;
using SafeWalk.Application.Interfaces.Persistence;
using System.Data;

namespace SafeWalk.Infrastructure.Persistence
{
    public class JourneyRepository : IJourneyRepository
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public JourneyRepository(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<JourneyDto?> GetByIdAsync(Guid journeyId, CancellationToken cancellationToken = default)
        {
            const string sql = @"
                SELECT Id, UserId, StartLocation, Destination,
                       StartTimeUtc, ExpectedArrivalTimeUtc, ActualArrivalTimeUtc, Status
                FROM Journeys
                WHERE Id = @Id";

            await using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync(cancellationToken);

            await using var command = new SqlCommand(sql, connection);
            command.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = journeyId });

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);
            if (!await reader.ReadAsync(cancellationToken))
            {
                return null;
            }

            var dto = new JourneyDto
            {
                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                UserId = reader.GetGuid(reader.GetOrdinal("UserId")),
                StartLocation = reader.GetString(reader.GetOrdinal("StartLocation")),
                Destination = reader.GetString(reader.GetOrdinal("Destination")),
                StartTimeUtc = reader.GetDateTime(reader.GetOrdinal("StartTimeUtc")),
                ExpectedArrivalTimeUtc = reader.GetDateTime(reader.GetOrdinal("ExpectedArrivalTimeUtc")),
                Status = reader.GetString(reader.GetOrdinal("Status"))
            };

            var actualArrivalOrdinal = reader.GetOrdinal("ActualArrivalTimeUtc");
            if (!reader.IsDBNull(actualArrivalOrdinal))
            {
                dto.ActualArrivalTimeUtc = reader.GetDateTime(actualArrivalOrdinal);
            }

            return dto;
        }

        public async Task<Guid> CreateAsync(JourneyDto journey, CancellationToken cancellationToken = default)
        {
            const string sql = @"
                INSERT INTO Journeys
                (Id, UserId, StartLocation, Destination,
                 StartTimeUtc, ExpectedArrivalTimeUtc, ActualArrivalTimeUtc, Status, CreatedAtUtc)
                VALUES
                (@Id, @UserId, @StartLocation, @Destination,
                 @StartTimeUtc, @ExpectedArrivalTimeUtc, @ActualArrivalTimeUtc, @Status, @CreatedAtUtc);";

            await using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync(cancellationToken);

            await using var command = new SqlCommand(sql, connection);
            command.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = journey.Id });
            command.Parameters.Add(new SqlParameter("@UserId", SqlDbType.UniqueIdentifier) { Value = journey.UserId });
            command.Parameters.Add(new SqlParameter("@StartLocation", SqlDbType.NVarChar, 500) { Value = journey.StartLocation });
            command.Parameters.Add(new SqlParameter("@Destination", SqlDbType.NVarChar, 500) { Value = journey.Destination });
            command.Parameters.Add(new SqlParameter("@StartTimeUtc", SqlDbType.DateTime2) { Value = journey.StartTimeUtc });
            command.Parameters.Add(new SqlParameter("@ExpectedArrivalTimeUtc", SqlDbType.DateTime2) { Value = journey.ExpectedArrivalTimeUtc });
            command.Parameters.Add(new SqlParameter("@ActualArrivalTimeUtc", SqlDbType.DateTime2)
            {
                Value = (object?)journey.ActualArrivalTimeUtc ?? DBNull.Value
            });
            command.Parameters.Add(new SqlParameter("@Status", SqlDbType.NVarChar, 50) { Value = journey.Status });
            command.Parameters.Add(new SqlParameter("@CreatedAtUtc", SqlDbType.DateTime2) { Value = DateTime.UtcNow });

            await command.ExecuteNonQueryAsync(cancellationToken);

            return journey.Id;
        }

        public async Task UpdateAsync(JourneyDto journey, CancellationToken cancellationToken = default)
        {
            const string sql = @"
                UPDATE Journeys
                SET StartLocation = @StartLocation,
                    Destination = @Destination,
                    StartTimeUtc = @StartTimeUtc,
                    ExpectedArrivalTimeUtc = @ExpectedArrivalTimeUtc,
                    ActualArrivalTimeUtc = @ActualArrivalTimeUtc,
                    Status = @Status,
                    UpdatedAtUtc = @UpdatedAtUtc
                WHERE Id = @Id;";

            await using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync(cancellationToken);

            await using var command = new SqlCommand(sql, connection);
            command.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = journey.Id });
            command.Parameters.Add(new SqlParameter("@StartLocation", SqlDbType.NVarChar, 500) { Value = journey.StartLocation });
            command.Parameters.Add(new SqlParameter("@Destination", SqlDbType.NVarChar, 500) { Value = journey.Destination });
            command.Parameters.Add(new SqlParameter("@StartTimeUtc", SqlDbType.DateTime2) { Value = journey.StartTimeUtc });
            command.Parameters.Add(new SqlParameter("@ExpectedArrivalTimeUtc", SqlDbType.DateTime2) { Value = journey.ExpectedArrivalTimeUtc });
            command.Parameters.Add(new SqlParameter("@ActualArrivalTimeUtc", SqlDbType.DateTime2)
            {
                Value = (object?)journey.ActualArrivalTimeUtc ?? DBNull.Value
            });
            command.Parameters.Add(new SqlParameter("@Status", SqlDbType.NVarChar, 50) { Value = journey.Status });
            command.Parameters.Add(new SqlParameter("@UpdatedAtUtc", SqlDbType.DateTime2) { Value = DateTime.UtcNow });

            await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }
}

