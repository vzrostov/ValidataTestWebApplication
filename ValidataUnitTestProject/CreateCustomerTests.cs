using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValidataTestWebApplication.DAL;
using ValidataTestWebApplication.Models;
using ValidataUnitTestProject;
using ValidataUnitTests.Helpers;

namespace ValidataUnitTests
{
    [TestFixture]
    internal class CreateCustomerTests : ValidataUnitTestBase
    {

        [SetUp]
        public void Setup()
        {
        }

        static IEnumerable<TestCaseData> GetAllTestCases
        {
            get
            {
                yield return new TestCaseData("Create customer in DB test", TestsHelper.GetTestCustomers(), TestsHelper.GetTestOrders(), TestsHelper.GetTestItems());
            }
        }

        [TestCaseSource("GetAllTestCases")]
        public void CreateCustomerTest(string description, List<Customer> inCustomerList, List<Order> inOrderList, List<Item> inItemsList)
        {
            // Arrange
            int prevCount = inCustomerList.Count;
            MockContext = MockCreateHelper.GetAsyncCustomerDbContextMock(inCustomerList.AsQueryable(), inOrderList.AsQueryable(), inItemsList.AsQueryable());
            UnitOfWork = new UnitOfWork(MockContext?.Object);
            // Act
            var task = UnitOfWork?.CreateCustomerAsync(new Customer("Johnny", "Depp", "USA", "413990", null));
            task?.ContinueWith(t =>
            {
                // Assert
                // reread all to check new size
                var curCount = UnitOfWork?.GetCustomers().Count();
                Assert.AreEqual(prevCount + 1, curCount, description);
            }
            );
        }
    }
}
