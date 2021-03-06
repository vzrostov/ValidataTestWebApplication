using Moq;
using NUnit.Framework;
using ValidataTest.Core.DAL;

namespace ValidataTests.UnitTests
{
    public class ValidataUnitTestBase
    {
        protected Mock<ICustomerDbContext>? MockContext { get; set; }
        protected UnitOfWork? UnitOfWork { get; set; }

        [TearDown]
        public void TearDown()
        {
            UnitOfWork?.Dispose();
            MockContext?.Object.Dispose();
        }
    }
}