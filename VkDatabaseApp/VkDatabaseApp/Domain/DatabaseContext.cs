using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using VkDatabaseDll.Domain.Entity;

namespace VkDatabaseDll.Domain
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("DefaultConnection") { }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<PostAttachment> PostAttachments { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Link> Links { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(u => u.VkId)
                .Property(u => u.VkId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<User>()
                .HasMany(x => x.Friends)
                .WithMany();

            modelBuilder.Entity<Post>()
                .HasKey(u => u.Id)
                .Property(u => u.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Photo>()
                .HasKey(u => u.Id)
                .Property(u => u.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}
