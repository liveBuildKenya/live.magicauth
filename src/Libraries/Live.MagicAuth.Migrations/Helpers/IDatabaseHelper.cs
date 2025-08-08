namespace Live.MagicAuth.Migrations.Helpers
{
    public interface IDatabaseHelper
    {
        void EnsureDatabaseExists(string connectionString);
    }
}
