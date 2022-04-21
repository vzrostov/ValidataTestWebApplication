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
    internal class GetOrderTests : ValidataUnitTestBase
    {
        internal struct DesiredResult
        {
            public int id;
            public DateTime date;
            public int itemCount;

            public DesiredResult()
            {
                this.id = 0;
                this.date = new DateTime();
                this.itemCount = 0;
            }

            public DesiredResult(int id, DateTime date, int itemCount)
            {
                this.id = id;
                this.date = date;
                this.itemCount = itemCount;
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
                yield return new TestCaseData("Get order from DB test", 1, true, new DesiredResult(1, new DateTime(2022, 04, 12, 2, 0, 0), 3), 24.5f,
                    TestsHelper.GetTestCustomers(), 
                    TestsHelper.GetOneTestOrderedCustomers().First().Orders, 
                    TestsHelper.GetTestItems(), 
                    TestsHelper.GetTestProducts());
            }
        }

        [TestCaseSource("GetAllTestCases")]
        public void GetCustomerTest(string description, int idOrder, bool isExist, DesiredResult result, float resultprice,
            List<Customer> inCustomerList, List<Order> inOrderList, List<Item> inItemsList, List<Product> inProductsList)
        {
            // Arrange
            MockContext = MockCreateHelper.GetAsyncDbContextMock(inCustomerList.AsQueryable(), inOrderList.AsQueryable(), inItemsList.AsQueryable(), inProductsList.AsQueryable());
            UnitOfWork = new UnitOfWork(MockContext?.Object);
            // Act
            var task = UnitOfWork?.GetOrderAsync(idOrder, "Items").ContinueWith(x =>
            {
                // Assert
                Assert.IsNotNull(x.Result, description);
                Assert.AreEqual(
                    result,
                    new DesiredResult(
                        x.Result.OrderId,
                        x.Result.Date,
                        x.Result.Items?.Count ?? 0),
                    description);
                Assert.AreEqual(resultprice, x.Result.TotalPrice, description + " Check TotalPrice calculating"); 
            });
            task?.Wait();
        }

        static IEnumerable<TestCaseData> GetAllWrongTestCases
        {
            get
            {
                yield return new TestCaseData(-8);
                yield return new TestCaseData(3333);
            }
        }

        [TestCaseSource("GetAllWrongTestCases")]
        public void GetNotExistingCustomerTest(int idOrder)
        {
            // Arrange
            MockContext = MockCreateHelper.GetAsyncDbContextMock(TestsHelper.GetTestCustomers().AsQueryable(), TestsHelper.GetTestOrders().AsQueryable(), TestsHelper.GetTestItems().AsQueryable());
            UnitOfWork = new UnitOfWork(MockContext?.Object);
            // Act 
            // Assert
            Assert.Throws<InvalidOperationException>(() => UnitOfWork?.GetOrderAsync(idOrder), "Get not existing order from DB test");
        }
    }


}
