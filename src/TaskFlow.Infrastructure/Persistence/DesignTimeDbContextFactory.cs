using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TaskFlow.Infrastructure.Persistence
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<TaskFlowDbContext>
    {
        public TaskFlowDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TaskFlowDbContext>();

            // Hardcoded connection string for migrations
            // This will NOT be used at runtime — only by EF tooling
            //optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TaskFlowDB;Integrated Security=True;Encrypt=True");
            optionsBuilder.UseSqlServer("Server = localhost,1433; Database = TaskFlowDB; User Id = SA; Password = YourStrong@Passw0rd; Encrypt = True; TrustServerCertificate = True;");
            // NEVER COMMIT CREDENTIALS, THIS IS TEMPORARY
            return new TaskFlowDbContext(optionsBuilder.Options);
        }
    }
}
