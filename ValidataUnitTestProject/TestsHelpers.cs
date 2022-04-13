using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using ValidataTestWebApplication.DAL;
using ValidataTestWebApplication.Models;

namespace ValidataUnitTests
{
    internal static class TestsHelpers
    {
        internal static List<Customer> GetTestCustomers()
        {
            return new List<Customer>
            {
                new Customer("John", "Twains", "USA", "43990", null) { CustomerID = 9 }, // has 3 orders
                new Customer("Sara", "Parker", "UK", "444990", null) { CustomerID = 10 },
                new Customer("Mark", "Twain", "USA", "43990", null) { CustomerID = 19 } // has 1 order
            };
        }

        internal static List<Customer> GetOneTestCustomer()
        {
            return new List<Customer>
            {
                GetTestCustomers().First()
            };
        }

        internal static List<Order> GetTestOrders()
        {
            return new List<Order>
            {
                new Order(new DateTime(2022, 04, 12, 2, 0, 0), price: 10f, customerID: 9, null, null) { OrderId = 1 },
                new Order(new DateTime(2022, 04, 12, 1, 0, 0), price: 20f, customerID: 9, null, null) { OrderId = 2 },
                new Order(new DateTime(2022, 04, 12, 3, 0, 0), price: 2f, customerID: 9, null, null) { OrderId = 3 },
                new Order(new DateTime(2022, 04, 12, 1, 0, 0), price: 20f, customerID: 19, null, null) { OrderId = 4 }
            };
        }

        internal static List<Item> GetTestItems()
        {
            return new List<Item>
            {
                new Item(quantity: 1.5f, productID: 1, null, orderID: 1, null) { ItemId = 1 },
                new Item(quantity: 10f, productID: 2, null, orderID: 2, null) { ItemId = 2 },
                new Item(quantity: 1f, productID: 3, null, orderID: 3, null) { ItemId = 3 },
                new Item(quantity: 1f, productID: 3, null, orderID: 4, null) { ItemId = 4 }
            };
        }

        internal static Mock<ICustomerDbContext> GetCustomerDbContextMock(IQueryable<Customer> data, IQueryable<Order>? dataOrder = null, IQueryable<Item>? dataItem = null)
        {
            var mockCustomerSet = new Mock<DbSet<Customer>>();
            mockCustomerSet.As<IQueryable<Customer>>().Setup(m => m.Provider).Returns(data.Provider);
            mockCustomerSet.As<IQueryable<Customer>>().Setup(m => m.Expression).Returns(data.Expression);
            mockCustomerSet.As<IQueryable<Customer>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockCustomerSet.As<IQueryable<Customer>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockOrderSet = new Mock<DbSet<Order>>();
            // order and items sets are excess sometimes
            if (dataOrder != null)
            {
                mockOrderSet.As<IQueryable<Order>>().Setup(m => m.Provider).Returns(dataOrder.Provider);
                mockOrderSet.As<IQueryable<Order>>().Setup(m => m.Expression).Returns(dataOrder.Expression);
                mockOrderSet.As<IQueryable<Order>>().Setup(m => m.ElementType).Returns(dataOrder.ElementType);
                mockOrderSet.As<IQueryable<Order>>().Setup(m => m.GetEnumerator()).Returns(dataOrder.GetEnumerator());
            }

            var mockItemSet = new Mock<DbSet<Item>>();
            // order and items sets are excess sometimes
            if (dataItem != null)
            {
                mockItemSet.As<IQueryable<Item>>().Setup(m => m.Provider).Returns(dataItem.Provider);
                mockItemSet.As<IQueryable<Item>>().Setup(m => m.Expression).Returns(dataItem.Expression);
                mockItemSet.As<IQueryable<Item>>().Setup(m => m.ElementType).Returns(dataItem.ElementType);
                mockItemSet.As<IQueryable<Item>>().Setup(m => m.GetEnumerator()).Returns(dataItem.GetEnumerator());
            }

            var mockDbContext = new Mock<DbContext>();
            mockDbContext.Setup(m => m.Set<Customer>()).Returns(mockCustomerSet.Object);
            if (dataOrder != null)
                mockDbContext.Setup(m => m.Set<Order>()).Returns(mockOrderSet.Object);
            if (dataItem != null)
                mockDbContext.Setup(m => m.Set<Item>()).Returns(mockItemSet.Object);

            var mockContext = new Mock<ICustomerDbContext>();
            mockContext.Setup(m => m.Customers).Returns(mockCustomerSet.Object);
            if (dataOrder != null)
                mockContext.Setup(m => m.Orders).Returns(mockOrderSet.Object);
            if (dataItem != null)
                mockContext.Setup(m => m.Items).Returns(mockItemSet.Object);
            mockContext.Setup(m => m.DBContext).Returns(mockDbContext.Object);
            return mockContext;
        }

        internal static Mock<ICustomerDbContext> GetAsyncCustomerDbContextMock(IQueryable<Customer> data, IQueryable<Order>? dataOrder = null, IQueryable<Item>? dataItem = null)
        {
            var mockCustomerSet = new Mock<DbSet<Customer>>();
            mockCustomerSet.As<IDbAsyncEnumerable<Customer>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<Customer>(data.GetEnumerator()));
            mockCustomerSet.As<IQueryable<Customer>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<Customer>(data.Provider));

            //mockCustomerSet.As<IQueryable<Customer>>().Setup(m => m.Provider).Returns(data.Provider);
            mockCustomerSet.As<IQueryable<Customer>>().Setup(m => m.Expression).Returns(data.Expression);
            mockCustomerSet.As<IQueryable<Customer>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockCustomerSet.As<IQueryable<Customer>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockOrderSet = new Mock<DbSet<Order>>();
            // order and items sets are excess sometimes
            if (dataOrder != null)
            {
                mockOrderSet.As<IDbAsyncEnumerable<Order>>()
                    .Setup(m => m.GetAsyncEnumerator())
                    .Returns(new TestDbAsyncEnumerator<Order>(dataOrder.GetEnumerator()));
                mockOrderSet.As<IQueryable<Order>>()
                    .Setup(m => m.Provider)
                    .Returns(new TestDbAsyncQueryProvider<Order>(dataOrder.Provider));
                mockOrderSet.As<IQueryable<Order>>().Setup(m => m.Provider).Returns(dataOrder.Provider);
                mockOrderSet.As<IQueryable<Order>>().Setup(m => m.Expression).Returns(dataOrder.Expression);
                mockOrderSet.As<IQueryable<Order>>().Setup(m => m.ElementType).Returns(dataOrder.ElementType);
                mockOrderSet.As<IQueryable<Order>>().Setup(m => m.GetEnumerator()).Returns(dataOrder.GetEnumerator());
            }

            var mockItemSet = new Mock<DbSet<Item>>();
            // order and items sets are excess sometimes
            if (dataItem != null)
            {
                mockItemSet.As<IDbAsyncEnumerable<Item>>()
                    .Setup(m => m.GetAsyncEnumerator())
                    .Returns(new TestDbAsyncEnumerator<Item>(dataItem.GetEnumerator()));
                mockItemSet.As<IQueryable<Item>>()
                    .Setup(m => m.Provider)
                    .Returns(new TestDbAsyncQueryProvider<Item>(dataItem.Provider));
                mockItemSet.As<IQueryable<Item>>().Setup(m => m.Provider).Returns(dataItem.Provider);
                mockItemSet.As<IQueryable<Item>>().Setup(m => m.Expression).Returns(dataItem.Expression);
                mockItemSet.As<IQueryable<Item>>().Setup(m => m.ElementType).Returns(dataItem.ElementType);
                mockItemSet.As<IQueryable<Item>>().Setup(m => m.GetEnumerator()).Returns(dataItem.GetEnumerator());
            }

            var mockDbContext = new Mock<DbContext>();
            mockDbContext.Setup(m => m.Set<Customer>()).Returns(mockCustomerSet.Object);
            if (dataOrder != null)
                mockDbContext.Setup(m => m.Set<Order>()).Returns(mockOrderSet.Object);
            if (dataItem != null)
                mockDbContext.Setup(m => m.Set<Item>()).Returns(mockItemSet.Object);

            var mockContext = new Mock<ICustomerDbContext>();
            mockContext.Setup(m => m.Customers).Returns(mockCustomerSet.Object);
            if (dataOrder != null)
                mockContext.Setup(m => m.Orders).Returns(mockOrderSet.Object);
            if (dataItem != null)
                mockContext.Setup(m => m.Items).Returns(mockItemSet.Object);
            mockContext.Setup(m => m.DBContext).Returns(mockDbContext.Object);
            return mockContext;
        }
    }

    internal class TestDbAsyncQueryProvider<TEntity> : IDbAsyncQueryProvider
    {
        private readonly IQueryProvider _inner;

        internal TestDbAsyncQueryProvider(IQueryProvider inner)
        {
            _inner = inner;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            return new TestDbAsyncEnumerable<TEntity>(expression);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new TestDbAsyncEnumerable<TElement>(expression);
        }

        public object Execute(Expression expression)
        {
            return _inner.Execute(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return _inner.Execute<TResult>(expression);
        }

        public Task<object> ExecuteAsync(Expression expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute(expression));
        }

        public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute<TResult>(expression));
        }
    }

    internal class TestDbAsyncEnumerable<T> : EnumerableQuery<T>, IDbAsyncEnumerable<T>, IQueryable<T>
    {
        public TestDbAsyncEnumerable(IEnumerable<T> enumerable)
            : base(enumerable)
        { }

        public TestDbAsyncEnumerable(Expression expression)
            : base(expression)
        { }

        public IDbAsyncEnumerator<T> GetAsyncEnumerator()
        {
            return new TestDbAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
        }

        IDbAsyncEnumerator IDbAsyncEnumerable.GetAsyncEnumerator()
        {
            return GetAsyncEnumerator();
        }

        IQueryProvider IQueryable.Provider
        {
            get { return new TestDbAsyncQueryProvider<T>(this); }
        }
    }

    internal class TestDbAsyncEnumerator<T> : IDbAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;

        public TestDbAsyncEnumerator(IEnumerator<T> inner)
        {
            _inner = inner;
        }

        public void Dispose()
        {
            _inner.Dispose();
        }

        public Task<bool> MoveNextAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_inner.MoveNext());
        }

        public T Current
        {
            get { return _inner.Current; }
        }

        object IDbAsyncEnumerator.Current
        {
            get { return Current; }
        }
    }
}