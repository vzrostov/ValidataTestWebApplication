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
    internal class GetCustomerTests
    {
        internal class DesiredResult
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
                //yield return new TestCaseData("No any customers in DB test", 9, null, new List<Customer> { });
                //yield return new TestCaseData("Only one customer in DB test", 9, new DesiredResult(1, "Twains", "Twains"), new List<Customer> { });
                yield return new TestCaseData("Many customers in DB test", 9, new DesiredResult(9, "Twains", "Twain"), TestsHelpers.GetTestCustomers());
                //yield return new TestCaseData("Many customers in DB test (we can't find)", 9, null, TestsHelpers.GetTestCustomers());
            }
        }

        [TestCaseSource("GetAllTestCases")]
        [Parallelizable(ParallelScope.All)]
        public void GetCustomerTest(string description, int idCustomer, DesiredResult result, List<Customer> inCustomerList)
        {
            GetCustomerTest2(description, idCustomer, result, inCustomerList);
        }

            public async void GetCustomerTest2(string description, int idCustomer, DesiredResult result, List<Customer> inCustomerList)
        {
            Mock<ICustomerDbContext> mockContext = TestsHelpers.GetAsyncCustomerDbContextMock(inCustomerList.AsQueryable());

            UnitOfWork unitOfWork = new UnitOfWork(mockContext.Object);
            //var tk = unitOfWork.GetCustomer(idCustomer);
            var task = await unitOfWork.GetCustomerAsync(idCustomer);
            //task.ContinueWith(x =>
            //{
            //    if (result == null) 
            //    {
            //        Assert.IsNull(x.Result);
            //        return; // check further only if we have info
            //    }
            //    //Assert.IsNotNull(x.Result);
            //    //Assert.AreEqual(
            //    //    result,
            //    //    new DesiredResult(
            //    //        x.Result.CustomerID,
            //    //        x.Result.FirstName,
            //    //        x.Result.LastName
            //    //        ),
            //    //    description);
            //}).
            //Wait();

            int o = 0;
            //unitOfWork.CreateCustomerAsync(new Customer("Margo", "Mount", "UK", "43990", new ReadOnlyCollection<Order>(new List<Order>())));

            //var tc = unitOfWork.GetCustomerAsync(9).
            //    ContinueWith(x =>
            //    {
            //        Assert.Equals(9, x.Result?.CustomerID ?? 0);
            //        unitOfWork.DeleteCustomerAsync(x.Result).ContinueWith(x =>
            //        {
            //            //Assert.Equals(9, x.Result?.CustomerID ?? 0);
            //            //Assert.Equals(9, x.Result.CustomerID);
            //        });
            //    });
            //tc.Wait(); // if it wait continuing?

        }
    }
}
