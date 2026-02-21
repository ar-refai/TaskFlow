using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Domain;
using TaskFlow.Domain.Entities;
using TaskEntity = TaskFlow.Domain.Entities.Task;

namespace TaskFlow.Infrastructure.Persistence
{
    public class TaskFlowDbContext : DbContext, IUnitOfWork
    {
        public TaskFlowDbContext(DbContextOptions<TaskFlowDbContext> options) : base(options)
        {

        }

        public DbSet<TaskEntity> Tasks => Set<TaskEntity>();
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<TeamMember> TeamMembers => Set<TeamMember>();
        public DbSet<Comment> Comments => Set<Comment>();

        public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await base.SaveChangesAsync(cancellationToken);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TaskFlowDbContext).Assembly);
        }

    }
}
