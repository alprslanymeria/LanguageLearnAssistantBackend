using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace App.Persistence;

/// <summary>
/// THIS ELIMINATES THE NEED TO EXECUTE Program.cs DURING MIGRATIONS.
/// EF TOOLS WILL USE THIS FACTORY INSTEAD OF BUILDING THE FULL APPLICATION HOST.
/// </summary>
public class AppDbContextDesignTimeFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        // BUILD CONFIGURATION FROM APPSETTINGS + ENVIRONMENT VARIABLES
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "API", "App.API"))
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("SqlServer")
            ?? throw new InvalidOperationException(
                "ConnectionString 'SqlServer' not found. " +
                "Set it via appsettings, user-secrets, or environment variable: ConnectionStrings__SqlServer");

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        DbContextConfigurator.Configure(optionsBuilder, connectionString, typeof(PersistenceAssembly).Assembly.FullName!);

        return new AppDbContext(optionsBuilder.Options);
    }
}
