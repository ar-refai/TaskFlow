using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Application.Projects.Commands.CreateProject;
using TaskFlow.Application.Projects.Queries;
using TaskFlow.Application.Tasks.Commands.CreateTask;
using TaskFlow.Application.Tasks.Queries;

namespace TaskFlow.Application
{
    public static class DependencyInjection
    {
        // this way is better
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<CreateTaskHandler>();
            services.AddScoped<GetTaskByIdHandler>();

            services.AddScoped<CreateProjectHandler>();
            services.AddScoped<GetProjectByIdHandler>();
            // As you add more handlers, register them here:
            // services.AddScoped<AssignTaskHandler>();
            // services.AddScoped<CompleteTaskHandler>();
            return services;
        }

        ////  can auto register
        //public static IServiceCollection AddApplication(this IServiceCollection services)
        //{
        //    var assemply = typeof(DependencyInjection).Assembly;

        //    var handlerTypes = assemply.GetTypes().Where(t => t.Name.EndsWith("Handler") && !t.IsInterface && !t.IsAbstract);

        //    foreach(var handler in handlerTypes)
        //    {
        //        services.AddScoped(handler);
        //    }
        //    return services;
        //}
    }
}
