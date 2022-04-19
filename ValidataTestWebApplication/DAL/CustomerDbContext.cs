using System.Data.Entity;
using ValidataTestWebApplication.Models;

namespace ValidataTestWebApplication.DAL
{
    public class CustomerDbContext : DbContext, ICustomerDbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Product> Products { get; set; }

        DbContext ICustomerDbContext.DBContext => this;

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Order>().(i => i.Customer)
            //    .WithMany(c => c.Invoices)
            //    .OnDelete(DeleteBehavior.ClientCascade);
        }

    }
}