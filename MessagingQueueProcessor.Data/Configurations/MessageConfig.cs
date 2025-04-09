using MessagingQueueProcessor.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MessagingQueueProcessor.Data.Configurations
{
    public class MessageConfig : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable("message");

            builder.Property(e => e.Id)
                .HasColumnName("id");

            builder.Property(e => e.Type)
                .HasColumnName("type")
                .HasConversion<int>();

            builder.Property(e => e.Status)
                .HasColumnName("status")
                .HasConversion<int>();

            builder.Property(e => e.Payload)
                .HasColumnName("payload");

            builder.Property(e => e.RetryCount)
                .HasColumnName("retry_count");

            builder.Property(e => e.CreatedAt)
                .HasColumnName("created_at");
        }
    }
}
