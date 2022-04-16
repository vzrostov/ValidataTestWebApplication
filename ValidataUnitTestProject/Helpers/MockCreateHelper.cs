using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValidataTestWebApplication.DAL;
using ValidataTestWebApplication.Models;

namespace ValidataUnitTests.Helpers
{
    internal static class MockCreateHelper
    {
        internal static Mock<ICustomerDbContext> GetCustomerDbContextMock(IQueryable<Customer> data, IQueryable<Order>? dataOrder = null, IQueryable<Item>? dataItem = null)
        {
            var mockCustomerSet = new Mock<DbSet<Customer>>();
            mockCustomerSet.As<IQueryable<Customer>>().Setup(m => m.Provider).Returns(data.Provider);
            mockCustomerSet.As<IQueryable<Customer>>().Setup(m => m.Expression).Returns(data.Expression);
            mockCustomerSet.As<IQueryable<Customer>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockCustomerSet.As<IQueryable<Customer>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            mockCustomerSet.Setup(x => x.FindAsync(It.IsAny<int>())).ReturnsAsync((int id) =>
            {
                return data.First(x => x.CustomerID == id);
            });
            mockCustomerSet.Setup(x => x.Include(It.IsAny<string>())).Returns((string path) =>
            {
                return mockCustomerSet.Object;
            });

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

        internal static Mock<ICustomerDbContext> GetAsyncCustomerDbContextMock(IQueryable<Customer> data, IQueryable<Order> dataOrder, IQueryable<Item> dataItem)
        {
            // customer
            var mockCustomerSet = new Mock<DbSet<Customer>>();
            mockCustomerSet.As<IDbAsyncEnumerable<Customer>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<Customer>(data.GetEnumerator()));
            mockCustomerSet.As<IQueryable<Customer>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<Customer>(data.Provider));

            mockCustomerSet.As<IQueryable<Customer>>().Setup(m => m.Expression).Returns(data.Expression);
            mockCustomerSet.As<IQueryable<Customer>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockCustomerSet.As<IQueryable<Customer>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            mockCustomerSet.Setup(x => x.FindAsync(It.IsAny<object[]>())).ReturnsAsync((object[] ids) =>
            {
                return data.First(x => x.CustomerID == (int)ids[0]);
            });
            mockCustomerSet.Setup(x => x.Include(It.IsAny<string>())).Returns((string path) =>
            {
                return mockCustomerSet.Object;
            });

            // order
            var mockOrderSet = new Mock<DbSet<Order>>();
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

            var mockItemSet = new Mock<DbSet<Item>>();
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

            var mockDbContext = new Mock<DbContext>();
            mockDbContext.Setup(m => m.Set<Customer>()).Returns(mockCustomerSet.Object);
            mockDbContext.Setup(m => m.Set<Order>()).Returns(mockOrderSet.Object);
            mockDbContext.Setup(m => m.Set<Item>()).Returns(mockItemSet.Object);

            var mockContext = new Mock<ICustomerDbContext>();
            mockContext.Setup(m => m.Customers).Returns(mockCustomerSet.Object);
            mockContext.Setup(m => m.Orders).Returns(mockOrderSet.Object);
            mockContext.Setup(m => m.Items).Returns(mockItemSet.Object);

            mockContext.Setup(m => m.DBContext).Returns(mockDbContext.Object);

            return mockContext;
        }

    }
}
