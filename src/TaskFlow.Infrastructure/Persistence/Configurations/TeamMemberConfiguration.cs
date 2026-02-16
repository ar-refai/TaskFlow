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
    public class TeamMemberConfiguration : IEntityTypeConfiguration<TeamMember>
    {
        public void Configure(EntityTypeBuilder<TeamMember> builder)
        {
            builder.ToTable("TeamMembers");
            builder.HasKey("Id");
            builder.Property(tm => tm.Id)
                .HasConversion(
                id => id.Value,
                value => new TeamMemberId(value)
                );
            builder.Property(tm => tm.Name)
                .HasField("_name")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(200)
                .IsRequired();
            builder.Property(tm => tm.Email)
                .HasField("_email")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasConversion(
                email => email.Value,
                value => new Email(value)
                )
                .HasMaxLength(255)
                .IsRequired();
            builder.Property(tm => tm.CreatedAt)
                .IsRequired();
            builder.HasIndex(tm => tm.Email)
                .IsUnique();
            builder.HasIndex(tm => tm.Name);
        }
    }
}
