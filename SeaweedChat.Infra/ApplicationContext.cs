using Microsoft.EntityFrameworkCore;
using SeaweedChat.Domain.Aggregates;
namespace SeaweedChat.Infra;

public class ApplicationContext : DbContext
{
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Chat> Chats => Set<Chat>();
    public DbSet<Session> Sessions => Set<Session>();
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
                .WithOne()
                .HasForeignKey(p => p.ChatId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        modelBuilder.Entity<User>(u =>
        {
            u.HasKey(p => p.Id);
        });
        modelBuilder.Entity<ChatMember>(m =>
        {
            m.HasKey(p => p.Id);
            m.Navigation(p => p.User).AutoInclude();
            m.HasOne(p => p.User)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);
        });
        modelBuilder.Entity<Session>(s =>
        {
            s.HasKey(p => p.Id);
            s.HasOne(p => p.Account)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);
        });
        modelBuilder.Entity<Message>(m => 
        {
            m.HasKey(p => p.Id);
            m.HasOne(p => p.Owner)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);
            m.HasOne(p => p.Chat)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);
            m.Property(p => p.Text).HasMaxLength(4096);
        });
    }
}