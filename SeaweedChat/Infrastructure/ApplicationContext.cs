using Microsoft.EntityFrameworkCore;
using Microsoft;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.User>()
                .Navigation(e => e.Chats)
                .AutoInclude();

            base.OnModelCreating(modelBuilder);
        }
    }
}
