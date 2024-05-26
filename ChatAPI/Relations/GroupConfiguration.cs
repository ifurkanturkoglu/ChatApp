using ChatAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatAPI.Relations
{
    public class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.Property(a => a.GroupName).HasColumnName("GroupName").IsRequired();

            builder.HasMany(a => a.Messages).WithOne(a => a.Group).HasForeignKey(a => a.GroupId).OnDelete(DeleteBehavior.Cascade).IsRequired(false);

            builder.HasMany(a => a.UserGroups).WithOne(a => a.Group).HasForeignKey(a => a.GroupId).OnDelete(DeleteBehavior.Cascade).IsRequired();
        }
    }
}
