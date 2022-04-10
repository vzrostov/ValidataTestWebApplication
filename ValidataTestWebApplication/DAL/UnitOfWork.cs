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
        private CustomerDbContext dbContext = new CustomerDbContext();
        private CommonRepository<Customer> customerRepository;
        private CommonRepository<Order> orderRepository;

        public IQueryable<Customer> GetCustomers(Expression<Func<Customer, bool>> filter = null,
            Func<IQueryable<Customer>, IOrderedQueryable<Customer>> orderBy = null,
            string includeProperties = "")
        {
            return customerRepository.GetAll(filter, orderBy, includeProperties);
        }

        public Task<Customer> GetCustomerAsync(int id)
        {
            return customerRepository.GetAsync(id);
        }

        public Task<int> CreateCustomerAsync(Customer customer)
        {
            customerRepository.Create(customer);
            return SaveChangesAsync();
        }

        public Task<int> UpdateCustomerAsync(Customer customer)
        {
            customerRepository.Update(customer);
            return SaveChangesAsync();
        }

        public Task<int> DeleteCustomerAsync(Customer customer)
        {
            customerRepository.Delete(customer);
            return SaveChangesAsync();
        }


        public IQueryable<Order> GetOrders(Expression<Func<Order, bool>> filter = null,
            Func<IQueryable<Order>, IOrderedQueryable<Order>> orderBy = null,
            string includeProperties = "")
        {
            return orderRepository.GetAll(filter, orderBy, includeProperties);
        }

        public Task<Order> GetOrderAsync(int id)
        {
            return orderRepository.GetAsync(id);
        }

        public Task<int> CreateOrderAsync(Order order)
        {
            order.Recalculate();
            orderRepository.Create(order);
            return SaveChangesAsync();
        }

        public Task<int> UpdateOrderAsync(Order order)
        {
            order.Recalculate();
            orderRepository.Update(order);
            return SaveChangesAsync();
        }

        public Task<int> DeleteOrderAsync(Order order)
        {
            orderRepository.Delete(order);
            return SaveChangesAsync();
        }

        private CommonRepository<Customer> CustomerRepository
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

        private CommonRepository<Order> OrderRepository
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

        private Task<int> SaveChangesAsync() 
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
        #endregion IDiposable
    }
}