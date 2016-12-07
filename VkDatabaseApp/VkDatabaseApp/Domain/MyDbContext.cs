using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkDatabaseApp.Domain.Entity;

namespace VkDatabaseApp.Domain
{
    class MyDbContext : DbContext
    {
        public MyDbContext() : base("DefaultConnection") { }
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Group> Groups { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(x => x.Friends)    
                .WithMany();                
        }
    }
}
