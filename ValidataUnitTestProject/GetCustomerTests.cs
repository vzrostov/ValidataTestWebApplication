//using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValidataTestWebApplication.DAL;
using ValidataTestWebApplication.Models;

namespace ValidataUnitTests
{
    //public class FakeDbSet<T> : IDbSet<T> where T : class
    //{
    //    ObservableCollection<T> _data;
    //    IQueryable _query;

    //    public FakeDbSet()
    //    {
    //        _data = new ObservableCollection<T>();
    //        _query = _data.AsQueryable();
    //    }

    //    public virtual T Find(params object[] keyValues)
    //    {
    //        throw new NotImplementedException("Derive from FakeDbSet<T> and override Find");
    //    }

    //    public T Add(T item)
    //    {
    //        _data.Add(item);
    //        return item;
    //    }

    //    public T Remove(T item)
    //    {
    //        _data.Remove(item);
    //        return item;
    //    }

    //    public T Attach(T item)
    //    {
    //        _data.Add(item);
    //        return item;
    //    }

    //    public T Detach(T item)
    //    {
    //        _data.Remove(item);
    //        return item;
    //    }

    //    public T Create()
    //    {
    //        return Activator.CreateInstance<T>();
    //    }

    //    public TDerivedEntity Create<TDerivedEntity>() where TDerivedEntity : class, T
    //    {
    //        return Activator.CreateInstance<TDerivedEntity>();
    //    }

    //    public ObservableCollection<T> Local
    //    {
    //        get { return _data; }
    //    }

    //    Type IQueryable.ElementType
    //    {
    //        get { return _query.ElementType; }
    //    }

    //    System.Linq.Expressions.Expression IQueryable.Expression
    //    {
    //        get { return _query.Expression; }
    //    }

    //    IQueryProvider IQueryable.Provider
    //    {
    //        get { return _query.Provider; }
    //    }

    //    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    //    {
    //        return _data.GetEnumerator();
    //    }

    //    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    //    {
    //        return _data.GetEnumerator();
    //    }
    //}

    [TestFixture]
    internal class GetCustomerTests
    {
        [SetUp]
        public void Setup()
        {
        }


        [Test]
        public void GetCustomerTest()
        {
            var data = new List<Customer>
            {
                new Customer("John", "Twain", "USA", "43990", new ReadOnlyCollection<Order>(new List<Order>())),
                new Customer("Marc", "Twain", "USA", "43990", new ReadOnlyCollection<Order>(new List<Order>())),
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Customer>>();
            mockSet.As<IQueryable<Customer>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Customer>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Customer>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Customer>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<ICustomerDbContext>();
            mockContext.Setup(m => m.Customers).Returns(mockSet.Object);

            UnitOfWork unitOfWork = new UnitOfWork(mockContext.Object);
            var customers = unitOfWork.GetCustomers();

            //var service = new BlogService(mockContext.Object);
            //service.AddBlog("ADO.NET Blog", "http://blogs.msdn.com/adonet");

            //mockSet.Verify(m => m.Add(It.IsAny<Blog>()), Times.Once());
            //mockContext.Verify(m => m.SaveChanges(), Times.Once());

            //Mock<CustomerDbContext> mock = new Mock<CustomerDbContext>();

            //var mock2 = new Mock<IDbContext>();
            //mock2.Setup(x => x.Set<Customer>())
            //    .Returns(new FakeDbSet<Customer>
            //    {
            //new User { ID = 1 }
            //    });
            //mock.Object.Customers.Add();

            //        var options = new DbContextOptionsBuilder<CustomerDbContext>()
            //.UseInMemoryDatabase(databaseName: "MovieListDatabase")
            //.Options;

            //        // Insert seed data into the database using one instance of the context
            //        using (var context = new CustomerDbContext(options))
            //        {
            //            //context.Movies.Add(new Movie { Id = 1, Title = "Movie 1", YearOfRelease = 2018, Genre = "Action" });
            //            //context.Movies.Add(new Movie { Id = 2, Title = "Movie 2", YearOfRelease = 2018, Genre = "Action" });
            //            //context.Movies.Add(nnew Movie { Id = 3, Title = "Movie 3", YearOfRelease = 2019, Genre = "Action"});
            //            context.SaveChanges();
            //        }

            //        // Use a clean instance of the context to run the test
            //        using (var context = new CustomerDbContext(options))
            //        {
            //            MovieRepository movieRepository = new MovieRepository(context);
            //            List<Movies> movies == movieRepository.GetAll()

            //        Assert.Equal(3, movies.Count);
            //        }



        }
    }
}
