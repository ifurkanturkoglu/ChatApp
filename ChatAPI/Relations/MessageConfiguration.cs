using ChatAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatAPI.Relations
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.Property(a => a.Text).HasColumnName("Text").IsRequired();

            builder.Property(a => a.MessageTime).HasColumnName("MessageTime").IsRequired();
        }
    }
}
