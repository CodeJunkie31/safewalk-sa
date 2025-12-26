using Microsoft.Data.SqlClient;

namespace SafeWalk.Infrastructure.Persistence
{
    public interface ISqlConnectionFactory
    {
        SqlConnection CreateConnection();
    }
}
