using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ValidataTestWebApplication.DAL;
using ValidataTestWebApplication.Models;
using ValidataUnitTestProject;
using ValidataUnitTests.Helpers;

namespace ValidataUnitTests
{
    internal class GetOrdersTests : ValidataUnitTestBase
    {
        internal struct DesiredResult
        {
            public int totalCount;
            public int firstId;
            public int lastId;

            public DesiredResult(int totalCount) : this()
            {
                this.totalCount = totalCount;
            }

            public DesiredResult(int totalCount, int firstId, int lastId)
            {
                this.totalCount = totalCount;
                this.firstId = firstId;
                this.lastId = lastId;
            }
        }

        [SetUp]
        public void Setup()
        {
        }

        static IOrderedQueryable<Order> OrderingMethodByDate(IQueryable<Order> query) =>
            query.OrderBy(c => c.Date);

        static IOrderedQueryable<Order> OrderingMethodByDateDescending(IQueryable<Order> query) =>
            query.OrderByDescending(c => c.Date);

        static Expression<Func<Order, bool>> FilterByPrice(float limit, int customerId) =>
            x => x.Price > limit && (x.CustomerID == customerId);

        static IEnumerable<TestCaseData> GetAllTestCases
        {
            get
            {
                yield return new TestCaseData("No any customer orders in DB test", new DesiredResult(0), 
                    9,
                    TestsHelper.GetTestCustomers(), 
                    new List<Order>(),
                    new List<Item>(),
                    null,
                    null
                    );
                yield return new TestCaseData("Many customer orders in DB test", new DesiredResult(3, 1, 3),
                    9,
                    TestsHelper.GetTestCustomers(),
                    TestsHelper.GetTestOrders(),
                    TestsHelper.GetTestItems(),
                    null,
                    null
                    );
                yield return new TestCaseData("One customer order in DB test", new DesiredResult(1, 4, 4),
                    19,
                    TestsHelper.GetTestCustomers(),
                    TestsHelper.GetTestOrders(),
                    TestsHelper.GetTestItems(),
                    null,
                    null
                    );
                yield return new TestCaseData("Many customer orders in DB test with ordering ASC", new DesiredResult(3, 2, 3),
                    9,
                    TestsHelper.GetTestCustomers(),
                    TestsHelper.GetTestOrders(),
                    TestsHelper.GetTestItems(),
                    null,
                    (Func<IQueryable<Order>, IOrderedQueryable<Order>>)OrderingMethodByDate
                    );
                yield return new TestCaseData("Many customer orders in DB test with ordering DESC", new DesiredResult(3, 3, 2),
                    9,
                    TestsHelper.GetTestCustomers(),
                    TestsHelper.GetTestOrders(),
                    TestsHelper.GetTestItems(),
                    null,
                    (Func<IQueryable<Order>, IOrderedQueryable<Order>>)OrderingMethodByDateDescending
                    );
                yield return new TestCaseData("Many customer orders in DB test with ordering ASC and filtering", new DesiredResult(2, 2, 1),
                    9,
                    TestsHelper.GetTestCustomers(),
                    TestsHelper.GetTestOrders(),
                    TestsHelper.GetTestItems(),
                    FilterByPrice(5f, 9),
                    (Func<IQueryable<Order>, IOrderedQueryable<Order>>)OrderingMethodByDate
                    );
                yield return new TestCaseData("Many customer orders in DB test with ordering DESC and filtering", new DesiredResult(2, 1, 2),
                    9,
                    TestsHelper.GetTestCustomers(),
                    TestsHelper.GetTestOrders(),
                    TestsHelper.GetTestItems(),
                    FilterByPrice(5f, 9),
                    (Func<IQueryable<Order>, IOrderedQueryable<Order>>)OrderingMethodByDateDescending
                    );
            }
        }

        [TestCaseSource("GetAllTestCases")]
        public void GetAllOrdersOfCustomerTest(string description, 
            DesiredResult result, 
            int customerId,
            List<Customer> customers, 
            List<Order> orders, 
            List<Item> items,
            Expression<Func<Order, bool>> filter,
            Func<IQueryable<Order>, IOrderedQueryable<Order>> orderBy)
        {
            // Arrange
            MockContext = MockCreateHelper.GetCustomerDbContextMock(customers.AsQueryable(), orders.AsQueryable(), items.AsQueryable());
            UnitOfWork unitOfWork = new UnitOfWork(MockContext.Object);
            Expression<Func<Order, bool>> expr = filter == null ? o => (o.CustomerID == customerId) : filter;
            // Act
            var resultOrders = unitOfWork.GetOrders(expr, orderBy);
            // Assert
            List<Order> ordersList = resultOrders.ToList();
            Assert.IsNotNull(ordersList, description);
            Assert.AreEqual(result.totalCount, ordersList.Count(), description);
            if (result.totalCount == 0) // check only if we have info
                return;
            Assert.AreEqual(
                result,
                new DesiredResult(
                    ordersList.Count(),
                    ordersList.First().OrderId,
                    ordersList.Last().OrderId
                ), description);

        }
    }
}
