using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace TaskFlow.Infrastructure.Persistence.Interceptors
{
    public class AuditInterceptor : SaveChangesInterceptor
    {
        private const string CreatedAtPropertyName = "CreatedAt";
        private const string UpdatedAtPropertyName = "UpdatedAt";
        private const string DeletedAtPropertyName = "DeletedAt";

        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            UpdateAuditFields(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            UpdateAuditFields(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private static void UpdateAuditFields(DbContext? context)
        {
            if (context is null)
                return;

            var entries = context.ChangeTracker.Entries();

            foreach (var entry in entries)
            {
                // Handle CreatedAt for new entities
                if (entry.State == EntityState.Added)
                {
                    var createdAtProperty = entry.Properties
                        .FirstOrDefault(p => p.Metadata.Name == CreatedAtPropertyName);

                    if (createdAtProperty is not null && createdAtProperty.CurrentValue is null)
                    {
                        createdAtProperty.CurrentValue = DateTime.UtcNow;
                    }
                }

                // Handle UpdatedAt for modified entities
                if (entry.State == EntityState.Modified)
                {
                    var updatedAtProperty = entry.Properties
                        .FirstOrDefault(p => p.Metadata.Name == UpdatedAtPropertyName);

                    if (updatedAtProperty is not null)
                    {
                        updatedAtProperty.CurrentValue = DateTime.UtcNow;
                    }
                }

                // Soft delete (prepared for future - we'll add IsDeleted property later)
                // if (entry.State == EntityState.Deleted)
                // {
                //     var isDeletedProperty = entry.Properties
                //         .FirstOrDefault(p => p.Metadata.Name == DeletedAtPropertyName);
                //
                //     if (isDeletedProperty is not null)
                //     {
                //         entry.State = EntityState.Modified;
                //         isDeletedProperty.CurrentValue = true;
                //     }
                // }
            }
        }
    }
}
