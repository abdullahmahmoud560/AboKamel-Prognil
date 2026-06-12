using dotenv.net;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Services.Infrastructure.DbContexts;

public class CapsulaDbContextFactory : IDesignTimeDbContextFactory<CapsulaDbContext>
{
    public CapsulaDbContext CreateDbContext(string[] args)
    {
        // Load environment variables from .env file
        // Check multiple possible locations for the .env file
        var solutionRoot = FindSolutionRoot();
        var possibleEnvPaths = new[]
        {
            Path.Combine(solutionRoot, ".env"),
            Path.Combine(solutionRoot, "src", "AboKamel.Api", ".env"),
            ".env"
        };

        DotEnv.Load(options: new DotEnvOptions(envFilePaths: possibleEnvPaths, trimValues: true, ignoreExceptions: true));

        // Build configuration from environment variables
        var configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .Build();

        // Get connection string
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        // Configure DbContextOptions
        var optionsBuilder = new DbContextOptionsBuilder<CapsulaDbContext>();
        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

        return new CapsulaDbContext(optionsBuilder.Options);
    }

    private static string FindSolutionRoot()
    {
        var directory = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (directory != null && !directory.GetFiles("*.sln").Any())
        {
            directory = directory.Parent;
        }
        return directory?.FullName ?? Directory.GetCurrentDirectory();
    }
}
