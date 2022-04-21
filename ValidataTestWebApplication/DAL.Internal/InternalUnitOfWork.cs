using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ValidataTest.Core.DAL;
using ValidataTest.Core.Models;

namespace ValidataTestWebApplication.DAL
{
    /// <summary>
    /// Provides API for inner using (to manipulate DB directly for admin (non public API), to CRUD data manually as a separate rows)
    /// </summary>
    internal class InternalUnitOfWork : IDisposable
    {
        private CustomerDbContext dbContext = new CustomerDbContext();
        private CommonRepository<Customer> customerRepository;
        private CommonRepository<Order> orderRepository;
        private CommonRepository<Item> itemRepository;
        private CommonRepository<Product> productRepository;

        public CustomerDbContext DbContext => dbContext; 

        public CommonRepository<Customer> CustomerRepository
        {
            get
            {
                if (this.customerRepository == null)
                {
                    this.customerRepository = new CommonRepository<Customer>(dbContext);
                }
                return customerRepository;
            }
        }

        public CommonRepository<Order> OrderRepository
        {
            get
            {
                if (this.orderRepository == null)
                {
                    this.orderRepository = new CommonRepository<Order>(dbContext);
                }
                return orderRepository;
            }
        }

        public CommonRepository<Item> ItemRepository
        {
            get
            {
                if (this.itemRepository == null)
                {
                    this.itemRepository = new CommonRepository<Item>(dbContext);
                }
                return itemRepository;
            }
        }

        public CommonRepository<Product> ProductRepository
        {
            get
            {
                if (this.productRepository == null)
                {
                    this.productRepository = new CommonRepository<Product>(dbContext);
                }
                return productRepository;
            }
        }

        public Task<int> SaveChangesAsync()
        {
            return dbContext.SaveChangesAsync();
        }

        #region IDiposable
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    dbContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}