using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ChatAPI.Models
{
    public class ChatDbContext : DbContext
    {
        public ChatDbContext context;

        public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<UserFriend> UserFriends { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
