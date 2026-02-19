using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Application.Projects.Commands.CreateProject;
using TaskFlow.Application.Projects.Commands.DeleteProject;
using TaskFlow.Application.Projects.Commands.UpdateProject;
using TaskFlow.Application.Projects.Queries;
using TaskFlow.Application.Tasks.Commands.AssignTask;
using TaskFlow.Application.Tasks.Commands.ChangeTaskStatus;
using TaskFlow.Application.Tasks.Commands.CreateTask;
using TaskFlow.Application.Tasks.Queries;
using TaskFlow.Application.TeamMembers.Commands.CreateTeamMember;
using TaskFlow.Application.TeamMembers.Queries;

namespace TaskFlow.Application
{
    public static class DependencyInjection
    {
        //this way is better
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Project handlers
            services.AddScoped<GetAllProjectsHandler>();
            services.AddScoped<GetProjectByIdHandler>();
            services.AddScoped<CreateProjectHandler>();
            services.AddScoped<UpdateProjectHandler>();
            services.AddScoped<DeleteProjectHandler>();

            // Task Handlers
            services.AddScoped<CreateTaskHandler>();
            services.AddScoped<GetTaskByIdHandler>();
            services.AddScoped<ChangeTaskStatusHandler>();
            services.AddScoped<AssignTaskHandler>();

            // Team Members 
            services.AddScoped<CreateTeamMemberHandler>();
            services.AddScoped<GetTeamMemberByIdHandler>();

            // As you add more handlers, register them here:
            return services;
        }

        //  can auto register
        //    public static IServiceCollection AddApplication(this IServiceCollection services)
        //    {
        //        var assemply = typeof(DependencyInjection).Assembly;

        //        var handlerTypes = assemply.GetTypes().Where(t => t.Name.EndsWith("Handler") && !t.IsInterface && !t.IsAbstract);

        //        foreach (var handler in handlerTypes)
        //        {
        //            services.AddScoped(handler);
        //        }
        //        return services;
        //    }
    }
}
