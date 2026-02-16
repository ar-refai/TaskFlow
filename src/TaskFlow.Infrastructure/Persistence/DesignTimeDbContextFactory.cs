using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TaskFlow.Infrastructure.Persistence
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<TaskFlowDbContext>
    {
        public TaskFlowDbContext CreateDbContext(string[] args)
        {
            var connectionString = GetConnectionString();

            var optionsBuilder = new DbContextOptionsBuilder<TaskFlowDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new TaskFlowDbContext(optionsBuilder.Options);
        }

        private static string GetConnectionString()
        {
            // Try multiple locations to find appsettings.json
            var possiblePaths = new[]
            {
                // 1. Run from solution root
                Path.Combine(Directory.GetCurrentDirectory(), "TaskFlow.API", "appsettings.json"),
                
                // 2. Run from Infrastructure project folder (most common for migrations)
                Path.Combine(Directory.GetCurrentDirectory(),"../TaskFlow.API/appsettings.json"),
                
                // 3. Run from API project folder
                Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"),
            };

            var settingsPath = possiblePaths.FirstOrDefault(File.Exists)
                ?? throw new FileNotFoundException(
                    $"appsettings.json not found for TaskFlow. Searched:\n{string.Join("\n", possiblePaths)}"
                );

            // Log so you know exactly which file EF is using
            Console.WriteLine($"[DesignTime] Using settings from: {settingsPath}");

            var configuration = new ConfigurationBuilder()
                .AddJsonFile(settingsPath, optional: false)
                .Build();

            return configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found in the located appsettings.json.");
        }
    }
}