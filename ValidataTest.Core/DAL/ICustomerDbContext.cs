using System.Data.Entity;
using System.Threading.Tasks;
using ValidataTest.Core.Models;

namespace ValidataTest.Core.DAL
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