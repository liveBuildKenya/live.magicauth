using FluentMigrator.Runner;
using Live.MagicAuth.Migrations.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Live.MagicAuth.Migrations.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddMagicAuthMigrations(this IServiceCollection servicesCollection, string connectionString)
        {
            servicesCollection.AddSingleton<IDatabaseHelper, DatabaseHelper>();

            servicesCollection.AddFluentMigratorCore()
                .ConfigureRunner(runnerBuilder => runnerBuilder
                    .AddPostgres()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(typeof(InitialCreate).Assembly).For.All())
                .AddLogging(logging => logging.AddFluentMigratorConsole());
        }
    }
}
