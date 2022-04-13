using System.Data.Entity;
using System.Threading.Tasks;
using ValidataTestWebApplication.Models;

namespace ValidataTestWebApplication.DAL
{

    public interface ICustomerDbContext
    {
        DbContext DBContext { get; }
        DbSet<Customer> Customers { get; set; }
        DbSet<Order> Orders { get; set; }
        DbSet<Item> Items { get; set; }
        DbSet<Product> Products { get; set; }

        void Dispose();
        Task<int> SaveChangesAsync();
    }
}