using global::OrderApplicaion.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace OrderApplicaion.Persistence
{
    public class OutBoxContext : DbContext
    {
        public OutBoxContext(DbContextOptions<OutBoxContext> options) : base(options)
        {
        }

        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<OutboxEventModel> OutboxEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed data for Orders
            modelBuilder.Entity<OrderModel>().HasData(
                new OrderModel { Id = 1, CustomerId = "Cust1", TotalAmount = 100.50m },
                new OrderModel { Id = 2, CustomerId = "Cust2", TotalAmount = 200.00m }
                // Add more seed data as required
            );

            // Seed data for OutboxEvents
            modelBuilder.Entity<OutboxEventModel>().HasData(
                new OutboxEventModel { Id = 1, EventId = Guid.NewGuid(), EventType = "Event1", EventData = "EventData1", CreatedOn = DateTime.UtcNow },
                new OutboxEventModel { Id = 2, EventId = Guid.NewGuid(), EventType = "Event2", EventData = "EventData2", CreatedOn = DateTime.UtcNow }
                // Add more seed data as required
            );
        }
    }
}
