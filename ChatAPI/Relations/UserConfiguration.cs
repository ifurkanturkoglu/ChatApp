using ChatAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatAPI.Relations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(a => a.Name).HasColumnName("Name").IsRequired().HasMaxLength(120);

            builder.HasMany(a => a.Messages).WithOne(a => a.User).HasForeignKey(a => a.UserId).OnDelete(DeleteBehavior.Cascade).IsRequired(false);

            builder.HasMany(a => a.UserGroups).WithOne(a => a.User).HasForeignKey(a => a.UserId).OnDelete(DeleteBehavior.Cascade).IsRequired(false);

            builder.HasMany(a => a.Friends).WithOne(a => a.User).HasForeignKey(a => a.UserId).IsRequired(false);
        }
    }
}
