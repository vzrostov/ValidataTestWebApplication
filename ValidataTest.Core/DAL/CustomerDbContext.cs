using System.Data.Entity;
using ValidataTest.Core.Models;

namespace ValidataTest.Core.DAL
{
    internal class CustomerDbContext : DbContext, ICustomerDbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Product> Products { get; set; }

        DbContext ICustomerDbContext.DBContext => this;

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Orders)
                .WithRequired(o => o.Customer)
                .WillCascadeOnDelete(true);
            modelBuilder.Entity<Order>()
                .HasMany(c => c.Items)
                .WithRequired(o => o.Order)
                .WillCascadeOnDelete(true);
        }

    }
}