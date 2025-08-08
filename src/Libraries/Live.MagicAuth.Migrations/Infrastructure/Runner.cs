using FluentMigrator.Runner;
using Live.MagicAuth.Migrations.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Live.MagicAuth.Migrations.Infrastructure
{
    public static class Runner
    {
        public static void ExecuteMigrations(IServiceProvider serviceProvider, string connectionString)
        {
            var dbHelper = serviceProvider.GetRequiredService<IDatabaseHelper>();
            dbHelper.EnsureDatabaseExists(connectionString);
            // Instantiate the runner
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

            // Execute the migrations
            runner.MigrateUp();
        }
    }
}
