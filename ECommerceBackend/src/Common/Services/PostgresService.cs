using Npgsql;

namespace ECommerce.Common.Services
{
    public class PostgresService
    {
        public NpgsqlConnection Connection { get; }

        public PostgresService(string connectionString)
        {
            Connection = new NpgsqlConnection(connectionString);
            Connection.Open();
        }
    }
}