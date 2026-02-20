using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Application.Projects.Commands.CreateProject;
using TaskFlow.Application.Projects.Commands.DeleteProject;
using TaskFlow.Application.Projects.Commands.UpdateProject;
using TaskFlow.Application.Projects.Queries.GetAllProjects;
using TaskFlow.Application.Projects.Queries.GetProjectById;
using TaskFlow.Application.Tasks.Commands.AddCommentToTask;
using TaskFlow.Application.Tasks.Commands.AddRemoveTagsInTask;
using TaskFlow.Application.Tasks.Commands.AssignTask;
using TaskFlow.Application.Tasks.Commands.ChangeTaskStatus;
using TaskFlow.Application.Tasks.Commands.CreateTask;
using TaskFlow.Application.Tasks.Commands.DeleteTask;
using TaskFlow.Application.Tasks.Commands.UpdateTask;
using TaskFlow.Application.Tasks.Queries.GetTaskById;
using TaskFlow.Application.Tasks.Queries.GetTasksByProject;
using TaskFlow.Application.Tasks.Queries.GetTasksWithFilters;
using TaskFlow.Application.TeamMembers.Commands.CreateTeamMember;
using TaskFlow.Application.TeamMembers.Commands.DeleteTeamMember;
using TaskFlow.Application.TeamMembers.Commands.UpdateTeamMember;
using TaskFlow.Application.TeamMembers.Queries.GetAllTeamMembers;
using TaskFlow.Application.TeamMembers.Queries.GetAllTeamMemberTasks;
using TaskFlow.Application.TeamMembers.Queries.GetTeamMemberById;

namespace TaskFlow.Application
{
    public static class DependencyInjection
    {
        //this way is better
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Project Handlers
            // - Queries
            services.AddScoped<GetAllProjectsHandler>();
            services.AddScoped<GetProjectByIdHandler>();
            // - Commands
            services.AddScoped<CreateProjectHandler>();
            services.AddScoped<UpdateProjectHandler>();
            services.AddScoped<DeleteProjectHandler>();

            // Task Handlers
            // - Queries
            services.AddScoped<GetTaskByIdHandler>();
            services.AddScoped<GetTasksByProjectHandler>();
            services.AddScoped<GetTasksWithFiltersHandler>();
            // - Commands
            services.AddScoped<ChangeTaskStatusHandler>();
            services.AddScoped<AssignTaskHandler>();
            services.AddScoped<AddRemoveTagsInTaskHandler>();
            services.AddScoped<AddCommentToTaskHandler>();
            services.AddScoped<CreateTaskHandler>();
            services.AddScoped<UpdateTaskHandler>();
            services.AddScoped<DeleteTaskHandler>();

            // Team Members Handlers
            // - Queries
            services.AddScoped<GetAllTeamMembersHandler>();
            services.AddScoped<GetAllTeamMemberTasksHandler>();
            services.AddScoped<GetTeamMemberByIdHandler>();
            // - Commands
            services.AddScoped<CreateTeamMemberHandler>();
            services.AddScoped<UpdateTeamMemberHandler>();
            services.AddScoped<DeleteTeamMemberHandler>();

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
