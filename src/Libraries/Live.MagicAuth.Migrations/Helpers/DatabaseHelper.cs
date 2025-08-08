using Microsoft.Extensions.Logging;
using Npgsql;

namespace Live.MagicAuth.Migrations.Helpers
{

    public class DatabaseHelper : IDatabaseHelper
    {
        private readonly ILogger<DatabaseHelper> logger;

        public DatabaseHelper(ILogger<DatabaseHelper> logger)
        {
            this.logger = logger;
        }
        public void EnsureDatabaseExists(string connectionString)
        {
            var builder = new NpgsqlConnectionStringBuilder(connectionString);
            var databaseName = builder.Database;

            // Connect to the default postgres database
            builder.Database = "postgres";

            using var connection = new NpgsqlConnection(builder.ConnectionString);
            connection.Open();

            // Check if the database exists
            using var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = "SELECT 1 FROM pg_database WHERE datname = @name";
            checkCmd.Parameters.AddWithValue("name", databaseName);

            var exists = checkCmd.ExecuteScalar() != null;

            if (!exists)
            {
                logger.LogInformation($"[INFO] Database '{databaseName}' does not exist. Creating...");

                using var createCmd = connection.CreateCommand();
                createCmd.CommandText = $"CREATE DATABASE \"{databaseName}\"";
                createCmd.ExecuteNonQuery();

                logger.LogInformation($"[INFO] Database '{databaseName}' created successfully.");
            }
            else
            {
                logger.LogInformation($"[INFO] Database '{databaseName}' already exists. Skipping creation.");
            }
        }
    }
}
