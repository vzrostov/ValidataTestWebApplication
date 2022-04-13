using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValidataTestWebApplication.DAL;
using ValidataTestWebApplication.Models;

namespace ValidataUnitTests
{
    [TestFixture]
    internal class CreateCustomerTests
    {

        [SetUp]
        public void Setup()
        {
        }

        static IEnumerable<TestCaseData> GetAllTestCasesCC
        {
            get
            {
                yield return new TestCaseData("Create customer in DB test", TestsHelpers.GetTestCustomers());
            }
        }

        [TestCaseSource("GetAllTestCasesCC")]
        [Parallelizable(ParallelScope.All)]
        public void CreateCustomerTest(string description, List<Customer> inCustomerList)
        {
            int prevCount = inCustomerList.Count;
            Mock<ICustomerDbContext> mockContext = TestsHelpers.GetAsyncCustomerDbContextMock(inCustomerList.AsQueryable());

            UnitOfWork unitOfWork = new UnitOfWork(mockContext.Object);
            var task = unitOfWork.CreateCustomerAsync(new Customer("Johnny", "Depp", "USA", "413990", null));
            //task.Start();
            task.ContinueWith(t =>
            {
                //Assert.True(task.Result > 0, description);
                // reread all to check new size
                var curCount = unitOfWork.GetCustomers().Count();
                Assert.AreEqual(prevCount + 1, curCount, description);
            }
            );
        }
    }
}
