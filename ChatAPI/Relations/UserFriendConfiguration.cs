using ChatAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatAPI.Relations
{
    public class UserFriendConfiguration : IEntityTypeConfiguration<UserFriend>
    {
        public void Configure(EntityTypeBuilder<UserFriend> builder)
        {
            builder.HasKey(a => new { a.UserId, a.FriendId });

            //builder.HasOne(a => a.User).WithMany(a=> a.Friends).HasForeignKey(a => a.UserId).IsRequired();

            //builder.HasOne(a => a.Friend).WithMany().HasForeignKey(a => a.FriendId).IsRequired();
        }
    }
}
