using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskEntity = TaskFlow.Domain.Entities.Task;
using TaskFlow.Domain.ValueObjects;
using TaskFlow.Domain.Entities;
namespace TaskFlow.Infrastructure.Persistence.Configurations
{

    public class TaskConfiguration : IEntityTypeConfiguration<TaskEntity>
    {
        public void Configure(EntityTypeBuilder<TaskEntity> builder)
        {
            // Table name
            builder.ToTable("Tasks");

            // Primary key
            builder.HasKey(t => t.Id);

            // ID conversion (TaskId value object → Guid in DB)
            builder.Property(t => t.Id)
                .HasConversion(
                    id => id.Value,           // To database: TaskId → Guid
                    value => new TaskId(value)) // From database: Guid → TaskId
                .ValueGeneratedNever();       // We generate IDs in domain, not DB

            // ProjectId conversion
            builder.Property(t => t.ProjectId)
                .HasConversion(
                    id => id.Value,
                    value => new ProjectId(value))
                .IsRequired();

            // Title (backing field)
            builder.Property(t => t.Title)
                .HasField("_title")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(200)
                .IsRequired();

            // Description (backing field, nullable)
            builder.Property(t => t.Description)
                .HasField("_description")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(2000);

            // Status (enum to string)
            builder.Property(t => t.TaskStatus)
                .HasField("_taskStatus")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            // Priority (enum to string)
            builder.Property(t => t.Priority)
                .HasField("_priority")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            // AssignedTo (nullable value object)
            builder.Property(t => t.AssignedTo)
                .HasField("_assignedTo")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasConversion(
                    id => id.HasValue ? id.Value.Value : (Guid?)null,
                    value => value.HasValue ? new TeamMemberId(value.Value) : null);

            // DateRange (owned entity)
            // DateRange (owned entity - struct version)
            builder.OwnsOne(t => t.DateRange, dateRange =>
            {
                dateRange.Property<DateTime>("StartDate")
                    .HasColumnName("StartDate");

                dateRange.Property<DateTime>("DueDate")
                    .HasColumnName("DueDate");
            });
           
            // Tags (many-to-many with value objects)
            builder.OwnsMany(t => t.Tags, tagsBuilder =>
            {
                //tagsBuilder.ToTable("TaskTags");

                //tagsBuilder.Property<Guid>("TaskEntityId");

                //tagsBuilder.Property(t => t.Value)
                //    .HasColumnName("TagValue")
                //    .HasMaxLength(30)
                //    .IsRequired();

                //tagsBuilder.HasKey("TaskEntityId", "Value");

                tagsBuilder.ToTable("TaskTags");
                tagsBuilder.WithOwner().HasForeignKey("TaskId");
                tagsBuilder.Property(t => t.Value)
                    .HasColumnName("TagValue")
                    .HasMaxLength(30)
                    .IsRequired();
                tagsBuilder.HasKey("TaskId", "Value");
            });

            // Comments (one-to-many)
            builder.HasMany<Comment>()
                .WithOne()
                .HasForeignKey(c => c.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            // Audit fields
            builder.Property(t => t.CreatedAt)
                .IsRequired();

            builder.Property(t => t.UpdatedAt);

            // Indexes for query performance
            builder.HasIndex(t => t.ProjectId);
            builder.HasIndex(t => t.TaskStatus);
            builder.HasIndex(t => t.AssignedTo);
        }
    }
}
