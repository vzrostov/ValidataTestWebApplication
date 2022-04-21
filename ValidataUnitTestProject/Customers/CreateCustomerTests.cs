using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValidataTest.Core.DAL;
using ValidataTest.Core.Models;
using ValidataUnitTests;
using ValidataUnitTests.Helpers;

namespace ValidataTests.UnitTests
{
    [TestFixture]
    internal class CreateCustomerTests : ValidataUnitTestBase
    {
        internal struct DesiredResult
        {
            public int id;
            public string firstName;
            public string lastName;

            public DesiredResult()
            {
                this.id = 0;
                this.firstName = string.Empty;
                this.lastName = string.Empty;
            }

            public DesiredResult(int id, string firstName, string lastName)
            {
                this.id = id;
                this.firstName = firstName;
                this.lastName = lastName;
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
                yield return new TestCaseData("Create customer in DB test", TestsHelper.GetTestCustomers(), TestsHelper.GetTestOrders(), TestsHelper.GetTestItems());
            }
        }

        [TestCaseSource("GetAllTestCases")]
        public void CreateCustomerTest(string description, List<Customer> inCustomerList, List<Order> inOrderList, List<Item> inItemsList)
        {
            // Arrange
            int prevCount = inCustomerList.Count;
            MockContext = MockCreateHelper.GetAsyncDbContextMock(inCustomerList.AsQueryable(), inOrderList.AsQueryable(), inItemsList.AsQueryable());
            UnitOfWork = new UnitOfWork(MockContext?.Object);
            var customer = new Customer("Johnny", "Depp", "USA", "413990", null) { CustomerID = 55 };
            // Act
            var task = UnitOfWork?.CreateCustomerAsync(customer).ContinueWith(t =>
            {
                // Assert
                Assert.IsNotNull(t.Result, description);
                // reread all to check new size
                var curCount = UnitOfWork?.GetCustomers().Count();
                Assert.AreEqual(prevCount + 1, curCount, description);
            }
            );
            task?.Wait();
        }
    }
}
