using Common.Model;
using Microsoft.EntityFrameworkCore;
using System;

namespace Infrastructure.Efcore
{
    public class ProjectDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public ProjectDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
