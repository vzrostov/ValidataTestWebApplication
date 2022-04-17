using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValidataTestWebApplication.DAL;
using ValidataTestWebApplication.Models;
using ValidataUnitTestProject;
using ValidataUnitTests;
using ValidataUnitTests.Helpers;

namespace ValidataTests.UnitTests.Orders
{
    [TestFixture]
    internal class GetOrderTests : ValidataUnitTestBase
    {
        internal struct DesiredResult
        {
            public int id;
            public DateTime date;
            public float price;
            public int itemCount;

            public DesiredResult()
            {
                this.id = 0;
                this.date = new DateTime();
                this.price = 0f;
                this.itemCount = 0;
            }

            public DesiredResult(int id, DateTime date, float price, int itemCount)
            {
                this.id = id;
                this.date = date;
                this.price = price;
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
                yield return new TestCaseData("Get order from DB test", 1, true, new DesiredResult(1, new DateTime(2022, 04, 12, 2, 0, 0), 10f, 3),
                    TestsHelper.GetTestCustomers(), TestsHelper.GetOneTestOrderedCustomers().First().Orders, TestsHelper.GetTestItems());
            }
        }

        [TestCaseSource("GetAllTestCases")]
        public void GetCustomerTest(string description, int idOrder, bool isExist, DesiredResult result, List<Customer> inCustomerList, List<Order> inOrderList, List<Item> inItemsList)
        {
            // Arrange
            MockContext = MockCreateHelper.GetAsyncDbContextMock(inCustomerList.AsQueryable(), inOrderList.AsQueryable(), inItemsList.AsQueryable());
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
                        x.Result.Price,
                        x.Result.Items?.Count ?? 0),
                    description);
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
