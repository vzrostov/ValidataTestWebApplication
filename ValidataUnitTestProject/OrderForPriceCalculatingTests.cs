using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ValidataTestWebApplication.Models;

namespace ValidataUnitTests
{
    [TestFixture]
    public class OrderForPriceCalculatingTests
    {
        [SetUp]
        public void Setup()
        {
        }

        public static IEnumerable<TestCaseData> CountOrderForPriceCalculatingTestCases
        {
            get
            {
                yield return new TestCaseData(0f, new Order(new DateTime(), 0f, 0, null, null));
                yield return new TestCaseData(0f, new Order(new DateTime(), 0f, 0, null, new ReadOnlyCollection<Item>(new List<Item>())));
                yield return new TestCaseData(0f, new Order(new DateTime(), 0f, 0, null,
                    new ReadOnlyCollection<Item>(
                        new List<Item>()
                        {
                            new Item()
                        }))
                );
                yield return new TestCaseData(0f, new Order(new DateTime(), 0f, 0, null,
                    new ReadOnlyCollection<Item>(
                        new List<Item>()
                        {
                            new Item(0f, 0, new Product(), 0, null)
                        }))
                );
                yield return new TestCaseData(0f, new Order(new DateTime(), 33.4f, 0, null, 
                    new ReadOnlyCollection<Item>(
                        new List<Item>()
                        {
                            new Item(0f, 0, new Product(null, 20), 0, null) 
                        }))
                );
                yield return new TestCaseData(0f, new Order(new DateTime(), 0f, 0, null, 
                    new ReadOnlyCollection<Item>(
                        new List<Item>()
                        {
                            new Item(20f, 0, new Product(null, 0), 0, null)
                        }))
                );
                yield return new TestCaseData(0f, new Order(new DateTime(), 0f, 0, null, 
                    new ReadOnlyCollection<Item>(
                        new List<Item>()
                        {
                            new Item(20f, 0, new Product(null, 0), 0, null)
                        }))
                );
                yield return new TestCaseData(204f, new Order(new DateTime(), 0f, 0, null, 
                    new ReadOnlyCollection<Item>(
                        new List<Item>()
                        {
                            new Item(20.4f, 0, new Product(null, 10), 0, null)
                        }))
                );
                yield return new TestCaseData(210.732f, new Order(new DateTime(), 33.4f, 0, null, 
                    new ReadOnlyCollection<Item>(
                        new List<Item>()
                        {
                            new Item(20.4f, 0, new Product(null, 10.33f), 0, null) 
                        }))
                );
                yield return new TestCaseData(311f, new Order(new DateTime(), 0f, 0, null, 
                    new ReadOnlyCollection<Item>(
                        new List<Item>()
                        {
                            new Item(10f, 0, new Product(null, 10.5f), 0, null),
                            new Item(01f, 0, new Product(null, 1f), 0, null),
                            new Item(10f, 0, new Product(null, 20.5f), 0, null)
                        }))
                );
            }

        }

        [TestCaseSource("CountOrderForPriceCalculatingTestCases")]
        [Parallelizable(ParallelScope.All)]
        public void OrderForPriceCalculatingTest(float result, Order order)
        {
            // Act
            order.Recalculate();
            // Assert
            Assert.AreEqual(result, order.Price);
        }

        [TearDown]
        public void TearDown()
        {
        }

    }
}