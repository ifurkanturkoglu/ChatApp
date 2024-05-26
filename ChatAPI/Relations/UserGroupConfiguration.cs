using ChatAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatAPI.Relations
{
    public class UserGroupConfiguration : IEntityTypeConfiguration<UserGroup>
    {
        public void Configure(EntityTypeBuilder<UserGroup> builder)
        {
            builder.HasKey(a => new { a.UserId, a.GroupId });
        }
    }
}
