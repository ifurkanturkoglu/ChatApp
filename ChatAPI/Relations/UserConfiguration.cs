using ChatAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatAPI.Relations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(a => a.UserName).HasColumnName("UserName").IsRequired().HasMaxLength(64);
            
            builder.Property(a => a.Name).HasColumnName("Name").IsRequired().HasMaxLength(120);
           
            builder.Property(a => a.Email).HasColumnName("Email").IsRequired().HasMaxLength(120);
            
            builder.Property(a => a.Password).HasColumnName("Password").IsRequired();

            builder.HasMany(a => a.Messages).WithOne(a => a.User).HasForeignKey(a => a.UserId).OnDelete(DeleteBehavior.Cascade).IsRequired(false);

            builder.HasMany(a => a.UserGroups).WithOne(a => a.User).HasForeignKey(a => a.UserId).OnDelete(DeleteBehavior.Cascade).IsRequired(false);

            builder.HasMany(a => a.Friends).WithOne(a => a.User).HasForeignKey(a => a.UserId).IsRequired(false);
        }
    }
}
