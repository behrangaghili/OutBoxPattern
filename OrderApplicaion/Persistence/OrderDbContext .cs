using global::OrderMicroservice.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace OrderMicroservice.Persistence
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
        {
        }

        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<OutboxEventModel> OutboxEvents { get; set; }
    }
}
