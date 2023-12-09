using global::OutBoxPattern.Models;
using Microsoft.EntityFrameworkCore;
using OutBoxPattern;
using System.Collections.Generic;
using System.Reflection.Emit;



namespace OutBoxPattern.Persistence



{
  

    namespace OutBoxPattern.Data
    {
        public class OutBoxContext : DbContext
        {
            public OutBoxContext(DbContextOptions<OutBoxContext> options) : base(options)
            {
            }

            public DbSet<Order> Orders { get; set; }
            public DbSet<OutboxEvent> OutboxEvents { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);

                 
            }
        }
    }

}
