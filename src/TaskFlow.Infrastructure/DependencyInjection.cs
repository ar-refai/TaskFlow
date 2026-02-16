using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Domain;
using TaskFlow.Domain.Repositories;
using TaskFlow.Infrastructure.Persistence;
using TaskFlow.Infrastructure.Persistence.Interceptors;
using TaskFlow.Infrastructure.Persistence.Repositories;

namespace TaskFlow.Infrastructure
{
    //public static class DependencyInjection
    //{
    //    public static IServiceCollection AddInfrastrpucture(this IServiceCollection services, IConfiguration configuration)
    //    {
    //        services.AddSingleton<AuditInterceptor>();
    //        services.AddDbContext<TaskFlowDbContext>((serviceProvider, options) =>
    //        {
    //            var connectionString = configuration.GetConnectionString("Server = localhost,1433; Database = TaskFlowDB; User Id = SA; Password = YourStrong@Passw0rd; Encrypt = True; TrustServerCertificate = True;") ?? throw new InvalidOperationException("connection string is not found");
    //            options.UseSqlServer(connectionString);
    //        });
    //        // Register IUnitOfWork as the same instance as DbContext
    //        services.AddScoped<IUnitOfWork>(serviceProvider =>
    //            serviceProvider.GetRequiredService<TaskFlowDbContext>());

    //        services.AddScoped<ITaskRepository, TaskRepository>();
    //        services.AddScoped<IProjectRepository, ProjectRepository>();
    //        services.AddScoped<ITeamMemberRepository, TeamMemberRepository>();
    //        return services;
    //    }
    //}
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // 1. Register the Interceptor
            services.AddSingleton<AuditInterceptor>();

            // 2. Configure DbContext
            services.AddDbContext<TaskFlowDbContext>((serviceProvider, options) =>
            {
                // This looks for the key under "ConnectionStrings" in your JSON
                var connectionString = configuration.GetConnectionString("DefaultConnection");

                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("Connection string 'DefaultConnection' was not found in configuration.");
                }

                options.UseSqlServer(connectionString);

                // 3. Link the Interceptor from DI to EF Core
                var interceptor = serviceProvider.GetRequiredService<AuditInterceptor>();
                options.AddInterceptors(interceptor);
            });

            // 4. Unit of Work & Repositories
            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<TaskFlowDbContext>());
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<ITeamMemberRepository, TeamMemberRepository>();

            return services;
        }
    }
    
       
}
