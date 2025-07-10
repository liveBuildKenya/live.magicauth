using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Live.MagicAuth.Domain.Infrastructure.Data
{
    /// <summary>
    /// Represents the migration runner
    /// </summary>
    public static class MigrationRunner
    {
        public static void RunMigrations(this IApplicationBuilder applicationBuilder)
        {
            using var scope = applicationBuilder.ApplicationServices.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<LiveDataProvider>();
            db.Database.Migrate();


        }
    }
}
