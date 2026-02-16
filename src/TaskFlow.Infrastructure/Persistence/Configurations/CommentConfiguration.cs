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
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("Comments");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id)
                .HasConversion(
                id => id.Value,
                value => new CommentId(value))
                .ValueGeneratedNever();
            builder.Property(c => c.TaskId)
                .HasConversion(
                id => id.Value,
                value => new TaskId(value));
            builder.Property(c => c.AuthorId)
                .HasConversion(
                id => id.Value,
                value => new TeamMemberId(value));
            builder.Property(c => c.Content)
                .HasField("_content")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(1000)
                .IsRequired();
            builder.Property(c => c.CreatedAt)
                .IsRequired();
            builder.HasIndex(c => c.AuthorId);
            builder.HasIndex(c => c.TaskId);
        }
    }
}
