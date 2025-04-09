using MessagingQueueProcessor.Data.Configurations;
using MessagingQueueProcessor.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace MessagingQueueProcessor.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public virtual DbSet<Message> Message { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new MessageConfig());
        }
    }
}
