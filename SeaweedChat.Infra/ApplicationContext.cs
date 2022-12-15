using Microsoft.EntityFrameworkCore;
using SeaweedChat.Domain.Aggregates;
namespace SeaweedChat.Infra;

public class ApplicationContext : DbContext
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Chat> Chats { get; set; }
    public DbSet<Message> Messages { get; set; }
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
}