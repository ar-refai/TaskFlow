using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Application.Tasks.Commands.CreateTask;
using TaskFlow.Application.Tasks.Queries;

namespace TaskFlow.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<CreateTaskHandler>();
            services.AddScoped<GetTaskByIdHandler>();
            // As you add more handlers, register them here:
            // services.AddScoped<AssignTaskHandler>();
            // services.AddScoped<CompleteTaskHandler>();
            return services;
        }
    }
}
