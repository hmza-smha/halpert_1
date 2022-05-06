using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace halpert.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRolesMapping> UserRolesMapping { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<WorksOn> WorksOn { get; set; }
        public DbSet<PredecessorTask> PredecessorTasks { get; set; }
        
        public ApplicationDbContext() : base("halpertDB")
        {
                
        }
    }
}