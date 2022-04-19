using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using ValidataTestWebApplication.DAL;
using ValidataTestWebApplication.Models;
using ValidataUnitTests;
using ValidataUnitTests.Helpers;

namespace ValidataTests.UnitTests
{
    [TestFixture]
    internal class DeleteOrderTests : ValidataUnitTestBase
    {

        [SetUp]
        public void Setup()
        {
        }

        static IEnumerable<TestCaseData> GetAllTestCases
        {
            get
            {
                yield return new TestCaseData("Delete order in DB test", TestsHelper.GetTestCustomers(), TestsHelper.GetTestOrders(), TestsHelper.GetTestOrders().First());
            }
        }

        [TestCaseSource("GetAllTestCases")]
        public void DeleteCustomerTest(string description, List<Customer> inCustomerList, List<Order> inOrderList, Order order)
        {
            // Arrange
            int prevCount = inOrderList.Count;
            MockContext = MockCreateHelper.GetAsyncDbContextMock(inCustomerList.AsQueryable(), inOrderList.AsQueryable());
            UnitOfWork = new UnitOfWork(MockContext?.Object);
            // Act
            var task = UnitOfWork?.DeleteOrderAsync(order).ContinueWith(t =>
            {
                // Assert
                // reread all to check new size
                var curCount = UnitOfWork?.GetOrders().Count();
                Assert.AreEqual(prevCount - 1, curCount, description);
            }
            );
            task?.Wait();
        }

        [Test]
        public void DeleteNotExistingOrderTest()
        {
            // Arrange
            int prevCount = TestsHelper.GetTestOrders().Count;
            MockContext = MockCreateHelper.GetAsyncDbContextMock(TestsHelper.GetTestCustomers().AsQueryable(), TestsHelper.GetTestOrders().AsQueryable(), null);
            UnitOfWork = new UnitOfWork(MockContext?.Object);
            Order order = TestsHelper.GetTestOrders().First();
            order.OrderId = 99; // set wrong Id
            // Act
            var task = UnitOfWork?.DeleteOrderAsync(order).ContinueWith(t =>
            {
                // Assert
                // reread all to check new size
                var curCount = UnitOfWork?.GetOrders().Count();
                Assert.AreEqual(prevCount, curCount, "Delete not existing order from DB test");
            }
            );
            task?.Wait();
        }
    }
}
