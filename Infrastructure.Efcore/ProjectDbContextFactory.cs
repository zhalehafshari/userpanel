
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Infrastructure.Efcore
{
    public class ProjectDbContextFactory : IDesignTimeDbContextFactory<ProjectDbContext>
    {
        public static ProjectDbContext CreateDbContext()
        {
            var OptionBuilder = new DbContextOptionsBuilder<ProjectDbContext>();
            var Connectionstringbuilder = new SqlConnectionStringBuilder()
            {
                DataSource = "DESKTOP-GSSPBO0",
                InitialCatalog = "WebSiteIdentity",
                IntegratedSecurity = true,
            };
            OptionBuilder.UseSqlServer(Connectionstringbuilder.ConnectionString);
            return new ProjectDbContext(OptionBuilder.Options);
        }
        public  ProjectDbContext CreateDbContext(string[] args)
        {
            return CreateDbContext();
        }
    }
}
