﻿//using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ValidataTestWebApplication.DAL;
using ValidataTestWebApplication.Models;
using ValidataUnitTestProject;
using ValidataUnitTests.Helpers;

namespace ValidataUnitTests
{
    [TestFixture]
    internal class GetCustomersTests : ValidataUnitTestBase
    {
        internal struct DesiredResult
        {
            public int totalCount;
            public int firstId;
            public string firstLastName;
            public int lastId;
            public string lastLastName;

            public DesiredResult(int totalCount)
            {
                this.totalCount = totalCount;
                this.firstId = 0;
                this.firstLastName = string.Empty;
                this.lastId = 0;
                this.lastLastName = string.Empty;
            }

            public DesiredResult(int totalCount, int firstId, string firstLastName, int lastId, string lastLastName)
            {
                this.totalCount = totalCount;
                this.firstId = firstId;
                this.firstLastName = firstLastName;
                this.lastId = lastId;
                this.lastLastName = lastLastName;
            }
        }

        [SetUp]
        public void Setup()
        {
        }

        static IOrderedQueryable<Customer> OrderingMethodByLastName(IQueryable<Customer> query) =>
            query.OrderBy(c => c.LastName);
        
        static IOrderedQueryable<Customer> OrderingMethodByLastNameDescending(IQueryable<Customer> query) =>
            query.OrderByDescending(c => c.LastName);

        static Expression<Func<Customer, bool>> FilterByFirstChar(char first) =>
            x => x.LastName.StartsWith(first);

        static IEnumerable<TestCaseData> GetAllTestCases
        {
            get
            {
                yield return new TestCaseData("No any customers in DB test", new DesiredResult(0), 
                    new List<Customer> { }, null, null);
                yield return new TestCaseData("Only one customer in DB test", new DesiredResult(1, 9, "Twains", 9, "Twains"), 
                    TestsHelper.GetOneTestCustomer(), null, null);
                yield return new TestCaseData("Many customers in DB test", new DesiredResult(3, 9, "Twains", 19, "Twain"), 
                    TestsHelper.GetTestCustomers(), null, null);
                yield return new TestCaseData("Many customers in DB test with ordering ASC", new DesiredResult(3, 10, "Parker", 9, "Twains"),
                    TestsHelper.GetTestCustomers(), null, (Func<IQueryable<Customer>, IOrderedQueryable<Customer>>) OrderingMethodByLastName);
                yield return new TestCaseData("Many customers in DB test with ordering DESC", new DesiredResult(3, 9, "Twains", 10, "Parker"),
                    TestsHelper.GetTestCustomers(), null, (Func<IQueryable<Customer>, IOrderedQueryable<Customer>>) OrderingMethodByLastNameDescending);
                yield return new TestCaseData("Many customers in DB test with ordering ASC and filtering", new DesiredResult(2, 19, "Twain", 9, "Twains"),
                    TestsHelper.GetTestCustomers(), FilterByFirstChar('T'), (Func<IQueryable<Customer>, IOrderedQueryable<Customer>>) OrderingMethodByLastName);
                yield return new TestCaseData("Many customers in DB test with ordering DESC and filtering", new DesiredResult(2, 9, "Twains", 19, "Twain"),
                    TestsHelper.GetTestCustomers(), FilterByFirstChar('T'), (Func<IQueryable<Customer>, IOrderedQueryable<Customer>>) OrderingMethodByLastNameDescending);
            }
        }

        [TestCaseSource("GetAllTestCases")]
        public void GetAllCustomersTest(string description, 
            DesiredResult result, 
            List<Customer> inCustomerList,
            Expression<Func<Customer, bool>> filter,
            Func<IQueryable<Customer>, IOrderedQueryable<Customer>> orderBy)
        {
            // Arrange
            MockContext = MockCreateHelper.GetCustomerDbContextMock(inCustomerList.AsQueryable());
            UnitOfWork unitOfWork = new UnitOfWork(MockContext.Object);
            // Act
            var customers = unitOfWork.GetCustomers(filter, orderBy);
            // Assert
            List<Customer> customersList = customers.ToList();
            Assert.IsNotNull(customersList, description);
            Assert.AreEqual(result.totalCount, customersList.Count(), description);
            if (result.totalCount == 0) // check only if we have info
                return;
            Assert.AreEqual(
                result, 
                new DesiredResult(
                    customersList.Count(), 
                    customersList.First().CustomerID,
                    customersList.First().LastName,
                    customersList.Last().CustomerID,
                    customersList.Last().LastName
                ), description);

        }

    }
}
