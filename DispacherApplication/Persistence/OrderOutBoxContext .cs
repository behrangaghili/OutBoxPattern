using global::DispacherApplication.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace DispacherApplication.Persistence
{
    public class OrderOutBoxContext : DbContext
    {
        public OrderOutBoxContext(DbContextOptions<OrderOutBoxContext> options) : base(options)
        {
        }

        public DbSet<OutboxEventModel> OutboxEvents { get; set; }

    }
}
