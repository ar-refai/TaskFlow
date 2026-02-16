using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.ValueObjects;

namespace TaskFlow.Infrastructure.Persistence.Configurations
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.ToTable("Projects");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                .HasConversion(
                    id => id.Value,
                    value => new ProjectId(value)
                ).ValueGeneratedNever();
            builder.Property(p => p.Name)
                .HasField("_name")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(p => p.Description)
                .HasField("_description")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(2000);

            builder.Property(p => p.CreatedAt).IsRequired();
            builder.Property(p => p.UpdatedAt);
            builder.HasIndex(p => p.Name);
        }

    }
}
