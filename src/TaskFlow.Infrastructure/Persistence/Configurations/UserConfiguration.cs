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
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id)
                .HasConversion(
                    id => id.Value,
                    value => new UserId(value)
                ).ValueGeneratedNever();
            builder.Property(tm => tm.Email)
                .HasField("_email")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasConversion(
                email => email.Value,
                value => new Email(value)
                );
            builder.HasIndex(u => u.Email).IsUnique();
            builder.Property(u => u.PasswordHash)
                .HasField("_passwordHash")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(u => u.Role)
                .HasField("_role")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(u => u.TeamMemberId)
                .HasConversion(
                    id => id.HasValue ? id.Value.Value : (Guid?)null,
                    value => value.HasValue ? new TeamMemberId(value.Value) : null);
            builder.Property(u => u.CreatedAt).IsRequired();
            builder.HasIndex(u => u.Role);

        }
    }
}
