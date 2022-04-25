using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace SeaweedChat.Infrastructure
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Models.User> Users { get; set; }
        public DbSet<Models.Message> Messages { get; set; }
        public DbSet<Models.Chat> Chats { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

       /* protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Chat>()
                .HasMany(t => t.Members)
                .WithMany(t => t.Chats)
                .UsingEntity(t => t.ToTable("ChatMembers"));
                
        }*/
    }
}
