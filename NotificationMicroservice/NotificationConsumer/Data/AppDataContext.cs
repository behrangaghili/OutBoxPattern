using Microsoft.EntityFrameworkCore;
using NotificationMicroservice.Entities;

namespace NotificationMicroservice.Data
{
    public class AppDataContext : DbContext
    {
        public AppDataContext(DbContextOptions<AppDataContext> options) : base(options)
        {

        }

        public DbSet<Message> Messages { get; set; }
        public DbSet<MessageTemplate> MessageTemplates { get; set; }
    }
}
