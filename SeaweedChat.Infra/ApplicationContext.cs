using Microsoft.EntityFrameworkCore;
using SeaweedChat.Domain.Aggregates;
namespace SeaweedChat.Infra;

public class ApplicationContext : DbContext
{
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Chat> Chats => Set<Chat>();
    public DbSet<Message> Messages => Set<Message>();
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(a =>
        {
            a.HasKey(p => p.Id);
            a.Property("_password");
        });
        modelBuilder.Entity<Chat>(c =>
        {
            c.HasKey(p => p.Id);
            c.Property("_members");
            c.Property("_messages");
        });
        modelBuilder.Entity<User>(u =>
        {
            u.HasKey(p => p.Id);
        });
        modelBuilder.Entity<Message>(m => 
        {
            m.HasKey(m => m.Id);
            m.Property(m => m.Chat);
            m.Property(m => m.Owner);
            m.Property(m => m.Text).HasMaxLength(4096);
        });
    }
}