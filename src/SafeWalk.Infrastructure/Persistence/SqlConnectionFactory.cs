using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace SafeWalk.Infrastructure.Persistence
{
    // IMPORTANT: this class MUST implement ISqlConnectionFactory
    public class SqlConnectionFactory : ISqlConnectionFactory
    {
        private readonly string _connectionString;

        public SqlConnectionFactory(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SafeWalkDb")
                ?? throw new InvalidOperationException("Connection string 'SafeWalkDb' not found.");
        }

        public SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}

