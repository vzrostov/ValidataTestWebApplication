using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using ValidataTestWebApplication.Models;

namespace ValidataTestWebApplication.DAL
{
    /// <summary>
    /// Provides API to manipulate customers and their orders through CustomerRepository and OrderRepository
    /// </summary>
    public class UnitOfWork : IDisposable
    {
        private ICustomerDbContext customerDbContext;
        private CommonRepository<Customer> customerRepository;
        private CommonRepository<Order> orderRepository;
        private bool IsForTesting { get; set; } = false;

        public UnitOfWork()
        {
            customerDbContext = new CustomerDbContext();
        }
        
        /// <summary>
        /// Constructor using for tests (possible to set mock dbcontext)
        /// </summary>
        /// <param name="dbctxt">Dbcontext, usually mock object</param>
        public UnitOfWork(ICustomerDbContext dbcontext)
        {
            customerDbContext = dbcontext;
            IsForTesting = true;
        }

        public IQueryable<Customer> GetCustomers(Expression<Func<Customer, bool>> filter = null,
            Func<IQueryable<Customer>, IOrderedQueryable<Customer>> orderBy = null,
            string includeProperties = "")
        {
            return CustomerRepository.GetAll(filter, orderBy, includeProperties);
        }

        public Task<Customer> GetCustomerAsync(int id, string includeProperties = "")
        {
            return CustomerRepository.GetAsync(id, includeProperties);
        }

        public Task<int> CreateCustomerAsync(Customer customer)
        {
            CustomerRepository.Create(customer);
            return SaveChangesAsync();
        }

        public Task<int> UpdateCustomerAsync(Customer customer)
        {
            CustomerRepository.Update(customer);
            return SaveChangesAsync();
        }

        public Task<int> DeleteCustomerAsync(Customer customer)
        {
            CustomerRepository.Delete(customer);
            return SaveChangesAsync();
        }

        public IQueryable<Order> GetOrders(Expression<Func<Order, bool>> filter = null,
            Func<IQueryable<Order>, IOrderedQueryable<Order>> orderBy = null,
            string includeProperties = "")
        {
            return OrderRepository.GetAll(filter, orderBy, includeProperties);
        }

        public Task<Order> GetOrderAsync(int id, string includeProperties = "")
        {
            return OrderRepository.GetAsync(id, includeProperties);
        }

        public Task<int> CreateOrderAsync(Order order)
        {
            order.Recalculate();
            OrderRepository.Create(order);
            return SaveChangesAsync();
        }

        public Task<int> UpdateOrderAsync(Order order)
        {
            order.Recalculate();
            OrderRepository.Update(order);
            return SaveChangesAsync();
        }

        public Task<int> DeleteOrderAsync(Order order)
        {
            OrderRepository.Delete(order);
            return SaveChangesAsync();
        }

        private CommonRepository<Customer> CustomerRepository
        {
            get
            {
                if (this.customerRepository == null)
                {
                    this.customerRepository = new CommonRepository<Customer>(customerDbContext);
                }
                return customerRepository;
            }
        }

        private CommonRepository<Order> OrderRepository
        {
            get
            {
                if (this.orderRepository == null)
                {
                    this.orderRepository = new CommonRepository<Order>(customerDbContext);
                }
                return orderRepository;
            }
        }

        private Task<int> SaveChangesAsync() 
        {
            return customerDbContext.SaveChangesAsync();
        }

        #region IDiposable
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    customerDbContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion IDiposable
    }
}