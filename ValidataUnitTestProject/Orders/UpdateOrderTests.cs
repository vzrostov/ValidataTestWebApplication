using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using ValidataTestWebApplication.DAL;
using ValidataTestWebApplication.Models;
using ValidataUnitTests;
using ValidataUnitTests.Helpers;

namespace ValidataTests.UnitTests
{
    [TestFixture]
    internal class UpdateOrderTests : ValidataUnitTestBase
    {
        internal struct DesiredResult
        {
            public int id;
            public DateTime date;

            public DesiredResult()
            {
                this.id = 0;
                this.date = new DateTime();
            }

            public DesiredResult(int id, DateTime date)
            {
                this.id = id;
                this.date = date;
            }
        }

        [SetUp]
        public void Setup()
        {
        }

        static IEnumerable<TestCaseData> GetAllTestCases
        {
            get
            {
                yield return new TestCaseData("Update order in DB test", TestsHelper.GetTestCustomers(), TestsHelper.GetTestOrders(), TestsHelper.GetTestOrders().First());
            }
        }

        [TestCaseSource("GetAllTestCases")]
        public void UpdateCustomerTest(string description, List<Customer> inCustomerList, List<Order> inOrderList, Order order)
        {
            // Arrange
            order.Date = DateTime.Now;
            MockContext = MockCreateHelper.GetAsyncDbContextMock(inCustomerList.AsQueryable(), inOrderList.AsQueryable());
            UnitOfWork = new UnitOfWork(MockContext?.Object);
            // Act
            var task = UnitOfWork?.UpdateOrderAsync(order).ContinueWith(t =>
            {
                // Assert
                Assert.IsNotNull(t.Result, description);
            }
            );
            task?.Wait();
            // find updated to check changes
            var newTask = UnitOfWork?.GetOrderAsync(order.OrderId).ContinueWith(t2 =>
            {
                Assert.IsNotNull(t2.Result, description);
                var newOrder = t2.Result;
                Assert.AreEqual(
                    new DesiredResult(
                        order.OrderId,
                        order.Date),
                    new DesiredResult(
                        newOrder.OrderId,
                        newOrder.Date),
                    description);
            });
            newTask?.Wait();
        }

        [Test]
        public void UpdateNotExistingCustomerTest()
        {
            // Arrange
            int prevCount = TestsHelper.GetTestCustomers().Count;
            MockContext = MockCreateHelper.GetAsyncDbContextMock(TestsHelper.GetTestCustomers().AsQueryable(), null, null);
            UnitOfWork = new UnitOfWork(MockContext?.Object);
            Customer customer = TestsHelper.GetOneTestCustomers().First();
            customer.CustomerID = 99; // set wrong Id
            // Act
            var task = UnitOfWork?.UpdateCustomerAsync(customer).ContinueWith(t =>
            {
                // Assert
                // reread all to check new size
                var curCount = UnitOfWork?.GetCustomers().Count();
                Assert.AreEqual(prevCount, curCount, "Update not existing customer from DB test");
            }
            );
            task?.Wait();
        }
    }
}
