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
            a.HasOne(p => p.User);
            a.Property("_password");
        });
        modelBuilder.Entity<Chat>(c =>
        {
            c.HasKey(p => p.Id);
            c.Property(p => p.Title).HasMaxLength(128);
            c.HasMany(p => p.Members)
                .WithMany()
                .UsingEntity("ChatMembers");
        });
        modelBuilder.Entity<User>(u =>
        {
            u.HasKey(p => p.Id);
        });
        modelBuilder.Entity<Message>(m => 
        {
            m.HasKey(p => p.Id);
            m.HasOne(p => p.Owner)
                .WithMany()
                .OnDelete(DeleteBehavior.ClientCascade);
            m.HasOne(p => p.Chat)
                .WithMany()
                .OnDelete(DeleteBehavior.ClientCascade);
            m.Property(p => p.Text).HasMaxLength(4096);
        });
    }
}