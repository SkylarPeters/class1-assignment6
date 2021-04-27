using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace cis237_assignment6.Models
{
    public partial class BeverageContext : DbContext
    {
        public BeverageContext()
            : base("name=BeverageContext")
        {
        }

        public virtual DbSet<Beverage> Beverages { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Beverage>()
                .Property(e => e.name)
                .IsFixedLength();

            modelBuilder.Entity<Beverage>()
                .Property(e => e.pack)
                .IsFixedLength();

            modelBuilder.Entity<Beverage>()
                .Property(e => e.price)
                .HasPrecision(19, 4);
        }
    }
}
