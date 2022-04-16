using Microsoft.EntityFrameworkCore;
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
    internal class GetCustomerTests : ValidataUnitTestBase
    {
        internal struct DesiredResult
        {
            public int id;
            public string firstName;
            public string lastName;
            public int orderCount;

            public DesiredResult()
            {
                this.id = 0;
                this.firstName = string.Empty;
                this.lastName = string.Empty;
                this.orderCount = 0;
            }

            public DesiredResult(int id, string firstName, string lastName, int orderCount)
            {
                this.id = id;
                this.firstName = firstName;
                this.lastName = lastName;
                this.orderCount = orderCount;
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
                yield return new TestCaseData("Get customer from DB test", 9, new DesiredResult(9, "John", "Twains", 3), 
                    TestsHelper.GetTestOrderedCustomers(), TestsHelper.GetTestOrders(), TestsHelper.GetTestItems());
                yield return new TestCaseData("Get customer from DB with only one user test", 9, new DesiredResult(9, "John", "Twains", 3), 
                    TestsHelper.GetOneTestOrderedCustomer(), TestsHelper.GetTestOrders(), TestsHelper.GetTestItems());
            }
        }

        [TestCaseSource("GetAllTestCases")]
        public void GetCustomerTest(string description, int idCustomer, DesiredResult result, List<Customer> inCustomerList, List<Order> inOrderList, List<Item> inItemsList)
        {
            // Arrange
            MockContext = MockCreateHelper.GetAsyncCustomerDbContextMock(inCustomerList.AsQueryable(), inOrderList.AsQueryable(), inItemsList.AsQueryable());
            UnitOfWork unitOfWork = new UnitOfWork(MockContext?.Object);
            // Act
            var task = unitOfWork.GetCustomerAsync(idCustomer, "Orders").ContinueWith(x =>
            {
                // Assert
                Assert.IsNotNull(x.Result, description);
                Assert.AreEqual(
                    result,
                    new DesiredResult(
                        x.Result.CustomerID,
                        x.Result.FirstName,
                        x.Result.LastName,
                        x.Result.Orders?.Count ?? 0), 
                    description);
            });
            task.Wait();
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
        public void GetNotExistingCustomerTest(int idCustomer)
        {
            // Arrange
            MockContext = MockCreateHelper.GetAsyncCustomerDbContextMock(TestsHelper.GetTestCustomers().AsQueryable(), TestsHelper.GetTestOrders().AsQueryable(), TestsHelper.GetTestItems().AsQueryable());
            UnitOfWork unitOfWork = new UnitOfWork(MockContext?.Object);
            // Act 
            // Assert
            Assert.Throws<InvalidOperationException>(() => unitOfWork.GetCustomerAsync(idCustomer), "Get not existing customer from DB test");
        }
    }
}
