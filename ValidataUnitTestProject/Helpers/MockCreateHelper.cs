using Moq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValidataTest.Core.DAL;
using ValidataTest.Core.Models;

namespace ValidataUnitTests.Helpers
{
    internal static class MockCreateHelper
    {
        internal static Mock<ICustomerDbContext> GetAsyncDbContextMock(IQueryable<Customer> data, 
            IQueryable<Order>? dataOrder = null, 
            IQueryable<Item>? dataItem = null,
            IQueryable<Product>? dataProduct = null)
        {
            // customer
            var mockCustomerSet = new Mock<DbSet<Customer>>();
            AssignDataInMockSet(mockCustomerSet, data);
            mockCustomerSet.Setup(x => x.FindAsync(It.IsAny<object[]>())).ReturnsAsync((object[] ids) =>
            {
                return data.First(x => x.CustomerID == (int)ids[0]);
            });
            mockCustomerSet.Setup(x => x.Include(It.IsAny<string>())).Returns((string path) =>
            {
                return mockCustomerSet.Object;
            });
            mockCustomerSet.Setup(x => x.Add(It.IsAny<Customer>())).Returns((Customer c) =>
            {
                var datanew = data.Append(c);
                AssignDataInMockSet(mockCustomerSet, datanew);
                return c;
            });
            mockCustomerSet.Setup(x => x.Attach(It.IsAny<Customer>())).Returns((Customer c) =>
            {
                data.ToList().ForEach(x => x.FirstName = (x.CustomerID == c.CustomerID) ? c.FirstName : x.FirstName);
                data.ToList().ForEach(x => x.LastName = (x.CustomerID == c.CustomerID) ? c.LastName : x.LastName);
                AssignDataInMockSet(mockCustomerSet, data.AsQueryable());
                return c;
            });
            mockCustomerSet.Setup(x => x.Remove(It.IsAny<Customer>())).Returns((Customer c) =>
            {
                var datanew = data.Where(x => x.CustomerID != c.CustomerID);
                AssignDataInMockSet(mockCustomerSet, datanew);
                return c;
            });

            // order
            var mockOrderSet = new Mock<DbSet<Order>>();
            // order and items sets are excess sometimes
            if (dataOrder != null)
            {
                AssignDataInMockSet(mockOrderSet, dataOrder);
                mockOrderSet.Setup(x => x.FindAsync(It.IsAny<object[]>())).ReturnsAsync((object[] ids) =>
                {
                    return dataOrder.First(x => x.OrderId == (int)ids[0]);
                });
                mockOrderSet.Setup(x => x.Include(It.IsAny<string>())).Returns((string path) =>
                {
                    return mockOrderSet.Object;
                });
                mockOrderSet.Setup(x => x.Add(It.IsAny<Order>())).Returns((Order c) =>
                {
                    var datanew = dataOrder.Append(c);
                    AssignDataInMockSet(mockOrderSet, datanew);
                    return c;
                });
                mockOrderSet.Setup(x => x.Attach(It.IsAny<Order>())).Returns((Order c) =>
                {
                    dataOrder.ToList().ForEach(x => x.Date = (x.OrderId == c.OrderId) ? c.Date : x.Date);
                    AssignDataInMockSet(mockOrderSet, dataOrder.AsQueryable());
                    return c;
                });
                mockOrderSet.Setup(x => x.Remove(It.IsAny<Order>())).Returns((Order c) =>
                {
                    var datanew = dataOrder.Where(x => x.OrderId != c.OrderId);
                    AssignDataInMockSet(mockOrderSet, datanew);
                    return c;
                });
            }

            // item
            var mockItemSet = new Mock<DbSet<Item>>();
            // order and items sets are excess sometimes
            if (dataItem != null)
            {
                AssignDataInMockSet(mockItemSet, dataItem);
            }

            // product
            var mockProductSet = new Mock<DbSet<Product>>();
            // order and items sets are excess sometimes
            if (dataProduct != null)
            {
                AssignDataInMockSet(mockProductSet, dataProduct);
            }

            // DbContext
            var mockDbContext = new Mock<DbContext>();
            mockDbContext.Setup(m => m.Set<Customer>()).Returns(mockCustomerSet.Object);
            if (dataOrder != null)
                mockDbContext.Setup(m => m.Set<Order>()).Returns(mockOrderSet.Object);
            if (dataItem != null)
                mockDbContext.Setup(m => m.Set<Item>()).Returns(mockItemSet.Object);
            if (dataProduct != null)
                mockDbContext.Setup(m => m.Set<Product>()).Returns(mockProductSet.Object);

            var mockContext = new Mock<ICustomerDbContext>();
            mockContext.Setup(m => m.SaveChangesAsync()).ReturnsAsync(() =>
            {
                return 0;
            });
            
            mockContext.Setup(m => m.Customers).Returns(mockCustomerSet.Object);
            if (dataOrder != null)
                mockContext.Setup(m => m.Orders).Returns(mockOrderSet.Object);
            if (dataItem != null)
                mockContext.Setup(m => m.Items).Returns(mockItemSet.Object);

            mockContext.Setup(m => m.DBContext).Returns(mockDbContext.Object);

            return mockContext;
        }

        private static void AssignDataInMockSet<T>(Mock<DbSet<T>> mockSet, IQueryable<T> data) where T : class
        {
            mockSet.As<IDbAsyncEnumerable<T>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<T>(data.GetEnumerator()));
            mockSet.As<IQueryable<T>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<T>(data.Provider));
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
        }
    }
}
