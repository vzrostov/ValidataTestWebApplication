﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
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

        /// <summary>
        /// Constructor for creating Unit of Work with Data Source to manipulate customers and their orders
        /// </summary>
        /// <example>var unitOfWork = new UnitOfWork();</example>
        public UnitOfWork()
        {
            customerDbContext = new CustomerDbContext();
        }

        /// <summary>
        /// Constructor using for tests (possible to set mock dbcontext)
        /// </summary>
        /// <param name="dbcontext">Dbcontext, usually mock object</param>
        public UnitOfWork(ICustomerDbContext dbcontext)
        {
            customerDbContext = dbcontext;
            IsForTesting = true;
        }

        /// <summary>
        /// Get Customers using settings for completeness of information
        /// </summary>
        /// <param name="filter">expression for selecting Customers</param>
        /// <param name="orderBy">function to order by rules</param>
        /// <param name="includeProperties">comma-separated lists of Customer properties to add in</param>
        /// <returns>Queries of Customers</returns>
        public IQueryable<Customer> GetCustomers(Expression<Func<Customer, bool>> filter = null,
            Func<IQueryable<Customer>, IOrderedQueryable<Customer>> orderBy = null,
            string includeProperties = "")
        {
            return CustomerRepository.GetAll(filter, orderBy, includeProperties);
        }

        /// <summary>
        /// Get Customer by ID using settings for completeness of information
        /// </summary>
        /// <param name="id">Id of Customer for search</param>
        /// <param name="includeProperties">comma-separated lists of Customer properties to add in</param>
        /// <returns>Asynchronous operation with result as Customer or null</returns>
        /// <exception cref="DbUpdateException">
        /// <see href="https://docs.microsoft.com/en-US/dotnet/api/microsoft.entityframeworkcore.dbcontext.savechangesasync?view=efcore-6.0"/>
        /// </exception>
        /// <exception cref="DbUpdateConcurrencyException">
        /// <see href="https://docs.microsoft.com/en-US/dotnet/api/microsoft.entityframeworkcore.dbcontext.savechangesasync?view=efcore-6.0"/>
        /// </exception>        
        /// <exception cref="OperationCanceledException">
        /// <see href="https://docs.microsoft.com/en-US/dotnet/api/microsoft.entityframeworkcore.dbcontext.savechangesasync?view=efcore-6.0"/>
        /// </exception>       
        public Task<Customer> GetCustomerAsync(int id, string includeProperties = "")
        {
            return CustomerRepository.GetAsync(id, includeProperties);
        }

        /// <summary>
        /// Create Customer in the database
        /// </summary>
        /// <param name="customer">Customer for creating</param>
        /// <returns>Asynchronous operation with result as the number of state entries written to the database</returns>
        /// <exception cref="DbUpdateException">
        /// <see href="https://docs.microsoft.com/en-US/dotnet/api/microsoft.entityframeworkcore.dbcontext.savechangesasync?view=efcore-6.0"/>
        /// </exception>
        /// <exception cref="DbUpdateConcurrencyException">
        /// <see href="https://docs.microsoft.com/en-US/dotnet/api/microsoft.entityframeworkcore.dbcontext.savechangesasync?view=efcore-6.0"/>
        /// </exception>        
        /// <exception cref="System.OperationCanceledException">
        /// <see href="https://docs.microsoft.com/en-US/dotnet/api/microsoft.entityframeworkcore.dbcontext.savechangesasync?view=efcore-6.0"/>
        /// </exception>
        /// <example>            
        /// UnitOfWork unitOfWork = new UnitOfWork();
        /// var customer = new Customer("test Name", "test LastName", "test Address", "test Code", null); /* create with no any orders */
        /// var result = await unitOfWork.CreateCustomerAsync(customer); /* "var customer" will have new CustomerID for possible operation further */
        /// </example>
        public Task<int> CreateCustomerAsync(Customer customer)
        {
            CustomerRepository.Create(customer);
            return SaveChangesAsync();
        }

        /// <summary>
        /// Update Customer in the database
        /// </summary>
        /// <param name="customer"></param>
        /// <returns>Asynchronous operation with result as the number of state entries written to the database</returns>
        /// <exception cref="DbUpdateException">
        /// <see href="https://docs.microsoft.com/en-US/dotnet/api/microsoft.entityframeworkcore.dbcontext.savechangesasync?view=efcore-6.0"/>
        /// </exception>
        /// <exception cref="DbUpdateConcurrencyException">
        /// <see href="https://docs.microsoft.com/en-US/dotnet/api/microsoft.entityframeworkcore.dbcontext.savechangesasync?view=efcore-6.0"/>
        /// </exception>        
        /// <exception cref="System.OperationCanceledException">
        /// <see href="https://docs.microsoft.com/en-US/dotnet/api/microsoft.entityframeworkcore.dbcontext.savechangesasync?view=efcore-6.0"/>
        /// </exception>       
        public Task<int> UpdateCustomerAsync(Customer customer)
        {
            CustomerRepository.Update(customer);
            return SaveChangesAsync();
        }

        /// <summary>
        /// Delete Customer from the database
        /// </summary>
        /// <param name="customer"></param>
        /// <returns>Asynchronous operation with result as the number of state entries written to the database</returns>
        /// <exception cref="DbUpdateException">
        /// <see href="https://docs.microsoft.com/en-US/dotnet/api/microsoft.entityframeworkcore.dbcontext.savechangesasync?view=efcore-6.0"/>
        /// </exception>
        /// <exception cref="DbUpdateConcurrencyException">
        /// <see href="https://docs.microsoft.com/en-US/dotnet/api/microsoft.entityframeworkcore.dbcontext.savechangesasync?view=efcore-6.0"/>
        /// </exception>        
        /// <exception cref="System.OperationCanceledException">
        /// <see href="https://docs.microsoft.com/en-US/dotnet/api/microsoft.entityframeworkcore.dbcontext.savechangesasync?view=efcore-6.0"/>
        /// </exception>       
        public Task<int> DeleteCustomerAsync(Customer customer)
        {
            CustomerRepository.Delete(customer);
            return SaveChangesAsync();
        }

        /// <summary>
        /// Get Orders using settings for completeness of information
        /// </summary>
        /// <param name="filter">expression for selecting Orders</param>
        /// <param name="orderBy">function to order by rules</param>
        /// <param name="includeProperties">comma-separated lists of Order properties to add in</param>
        /// <returns>Queries of Orders</returns>
        public IQueryable<Order> GetOrders(Expression<Func<Order, bool>> filter = null,
            Func<IQueryable<Order>, IOrderedQueryable<Order>> orderBy = null,
            string includeProperties = "")
        {
            return OrderRepository.GetAll(filter, orderBy, includeProperties);
        }

        /// <summary>
        /// Get Order by ID using settings for completeness of information
        /// </summary>
        /// <param name="id">Id of Order for search</param>
        /// <param name="includeProperties">comma-separated lists of Order properties to add in</param>
        /// <returns>Asynchronous operation with result as Order or null</returns>
        /// <exception cref="DbUpdateException">
        /// <see href="https://docs.microsoft.com/en-US/dotnet/api/microsoft.entityframeworkcore.dbcontext.savechangesasync?view=efcore-6.0"/>
        /// </exception>
        /// <exception cref="DbUpdateConcurrencyException">
        /// <see href="https://docs.microsoft.com/en-US/dotnet/api/microsoft.entityframeworkcore.dbcontext.savechangesasync?view=efcore-6.0"/>
        /// </exception>        
        /// <exception cref="System.OperationCanceledException">
        /// <see href="https://docs.microsoft.com/en-US/dotnet/api/microsoft.entityframeworkcore.dbcontext.savechangesasync?view=efcore-6.0"/>
        /// </exception>       
        public Task<Order> GetOrderAsync(int id, string includeProperties = "")
        {
            return OrderRepository.GetAsync(id, includeProperties);
        }

        /// <summary>
        /// Create Order in the database
        /// </summary>
        /// <param name="order">Order for creating</param>
        /// <returns>Asynchronous operation with result as the number of state entries written to the database</returns>
        /// <exception cref="DbUpdateException">
        /// <see href="https://docs.microsoft.com/en-US/dotnet/api/microsoft.entityframeworkcore.dbcontext.savechangesasync?view=efcore-6.0"/>
        /// </exception>
        /// <exception cref="DbUpdateConcurrencyException">
        /// <see href="https://docs.microsoft.com/en-US/dotnet/api/microsoft.entityframeworkcore.dbcontext.savechangesasync?view=efcore-6.0"/>
        /// </exception>        
        /// <exception cref="System.OperationCanceledException">
        /// <see href="https://docs.microsoft.com/en-US/dotnet/api/microsoft.entityframeworkcore.dbcontext.savechangesasync?view=efcore-6.0"/>
        /// </exception>       
        public Task<int> CreateOrderAsync(Order order)
        {
            OrderRepository.Create(order);
            return SaveChangesAsync();
        }

        /// <summary>
        /// Update Order in the database
        /// </summary>
        /// <param name="order"></param>
        /// <returns>Asynchronous operation with result as the number of state entries written to the database</returns>
        /// <exception cref="DbUpdateException">
        /// <see href="https://docs.microsoft.com/en-US/dotnet/api/microsoft.entityframeworkcore.dbcontext.savechangesasync?view=efcore-6.0"/>
        /// </exception>
        /// <exception cref="DbUpdateConcurrencyException">
        /// <see href="https://docs.microsoft.com/en-US/dotnet/api/microsoft.entityframeworkcore.dbcontext.savechangesasync?view=efcore-6.0"/>
        /// </exception>        
        /// <exception cref="System.OperationCanceledException">
        /// <see href="https://docs.microsoft.com/en-US/dotnet/api/microsoft.entityframeworkcore.dbcontext.savechangesasync?view=efcore-6.0"/>
        /// </exception>       
        public Task<int> UpdateOrderAsync(Order order)
        {
            OrderRepository.Update(order);
            return SaveChangesAsync();
        }

        /// <summary>
        /// Delete Customer from the database
        /// </summary>
        /// <param name="order"></param>
        /// <returns>Asynchronous operation with result as the number of state entries written to the database</returns>
        /// <exception cref="DbUpdateException">
        /// <see href="https://docs.microsoft.com/en-US/dotnet/api/microsoft.entityframeworkcore.dbcontext.savechangesasync?view=efcore-6.0"/>
        /// </exception>
        /// <exception cref="DbUpdateConcurrencyException">
        /// <see href="https://docs.microsoft.com/en-US/dotnet/api/microsoft.entityframeworkcore.dbcontext.savechangesasync?view=efcore-6.0"/>
        /// </exception>        
        /// <exception cref="System.OperationCanceledException">
        /// <see href="https://docs.microsoft.com/en-US/dotnet/api/microsoft.entityframeworkcore.dbcontext.savechangesasync?view=efcore-6.0"/>
        /// </exception>       
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