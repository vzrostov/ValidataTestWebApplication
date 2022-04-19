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
    internal class DeleteCustomerTests : ValidataUnitTestBase
    {

        [SetUp]
        public void Setup()
        {
        }

        static IEnumerable<TestCaseData> GetAllTestCases
        {
            get
            {
                yield return new TestCaseData("Delete customer in DB test", TestsHelper.GetTestCustomers(), TestsHelper.GetOneTestCustomers().First());
            }
        }

        [TestCaseSource("GetAllTestCases")]
        public void DeleteCustomerTest(string description, List<Customer> inCustomerList, Customer customer)
        {
            // Arrange
            int prevCount = inCustomerList.Count;
            MockContext = MockCreateHelper.GetAsyncDbContextMock(inCustomerList.AsQueryable());
            UnitOfWork = new UnitOfWork(MockContext?.Object);
            // Act
            var task = UnitOfWork?.DeleteCustomerAsync(customer).ContinueWith(t =>
            {
                // Assert
                // reread all to check new size
                var curCount = UnitOfWork?.GetCustomers().Count();
                Assert.AreEqual(prevCount - 1, curCount, description);
            }
            );
            task?.Wait();
        }

        [Test]
        public void DeleteNotExistingCustomerTest()
        {
            // Arrange
            int prevCount = TestsHelper.GetTestCustomers().Count;
            MockContext = MockCreateHelper.GetAsyncDbContextMock(TestsHelper.GetTestCustomers().AsQueryable(), null, null);
            UnitOfWork = new UnitOfWork(MockContext?.Object);
            Customer customer = TestsHelper.GetOneTestCustomers().First();
            customer.CustomerID = 99; // set wrong Id
            // Act
            var task = UnitOfWork?.DeleteCustomerAsync(customer).ContinueWith(t =>
            {
                // Assert
                // reread all to check new size
                var curCount = UnitOfWork?.GetCustomers().Count();
                Assert.AreEqual(prevCount, curCount, "Delete not existing customer from DB test");
            }
            );
            task?.Wait();
        }
    }
}
