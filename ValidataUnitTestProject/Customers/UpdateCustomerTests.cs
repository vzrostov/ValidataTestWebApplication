using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using ValidataTest.Core.DAL;
using ValidataTest.Core.Models;
using ValidataUnitTests;
using ValidataUnitTests.Helpers;

namespace ValidataTests.UnitTests
{
    [TestFixture]
    internal class UpdateCustomerTests : ValidataUnitTestBase
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
                yield return new TestCaseData("Update customer in DB test", TestsHelper.GetTestCustomers(), TestsHelper.GetOneTestCustomers().First());
            }
        }

        [TestCaseSource("GetAllTestCases")]
        public void UpdateCustomerTest(string description, List<Customer> inCustomerList, Customer customer)
        {
            // Arrange
            customer.FirstName = "Bob";
            customer.LastName = "Robbins";
            MockContext = MockCreateHelper.GetAsyncDbContextMock(inCustomerList.AsQueryable());
            UnitOfWork = new UnitOfWork(MockContext?.Object);
            // Act
            var task = UnitOfWork?.UpdateCustomerAsync(customer).ContinueWith(t =>
            {
                // Assert
                Assert.IsNotNull(t.Result, description);
            }
            );
            task?.Wait();
            // find updated to check changes
            var newTask = UnitOfWork?.GetCustomerAsync(customer.CustomerID).ContinueWith(t2 =>
            {
                Assert.IsNotNull(t2.Result, description);
                var newCustomer = t2.Result;
                Assert.AreEqual(
                    new DesiredResult(
                        customer.CustomerID,
                        customer.FirstName,
                        customer.LastName),
                    new DesiredResult(
                        newCustomer.CustomerID,
                        newCustomer.FirstName,
                        newCustomer.LastName),
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