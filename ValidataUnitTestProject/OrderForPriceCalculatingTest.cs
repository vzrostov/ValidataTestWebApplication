using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ValidataTestWebApplication.Models;

namespace ValidataUnitTestProject
{
    [TestFixture]
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        public static IEnumerable<TestCaseData> CountOrderForPriceCalculatingTestCases
        {
            get
            {
                yield return new TestCaseData(0f, new Order() { Items = null });
                yield return new TestCaseData(0f, new Order() { Items = new ReadOnlyCollection<Item>(new List<Item>()) });
                yield return new TestCaseData(0f, new Order()
                {
                    Items = new ReadOnlyCollection<Item>(
                        new List<Item>()
                        {
                            new Item()
                        })
                });
                yield return new TestCaseData(0f, new Order()
                {
                    Items = new ReadOnlyCollection<Item>(
                        new List<Item>()
                        {
                            new Item() { Quantity = 0, Product = new Product() { } }
                        })
                });
                yield return new TestCaseData(0f, new Order()
                {
                    Price = 33.4f,
                    Items = new ReadOnlyCollection<Item>(
                        new List<Item>()
                        {
                            new Item() { Quantity = 0, Product = new Product() { Price = 20 } }
                        })
                });
                yield return new TestCaseData(0f, new Order()
                {
                    Items = new ReadOnlyCollection<Item>(
                        new List<Item>()
                        {
                            new Item() { Quantity = 20, Product = new Product() { Price = 0 } }
                        })
                });
                yield return new TestCaseData(0f, new Order()
                {
                    Items = new ReadOnlyCollection<Item>(
                        new List<Item>()
                        {
                            new Item() { Quantity = 20, Product = new Product() { Price = 0 } }
                        })
                });
                yield return new TestCaseData(204f, new Order()
                {
                    Items = new ReadOnlyCollection<Item>(
                        new List<Item>()
                        {
                            new Item() { Quantity = 20.4f, Product = new Product() { Price = 10 } }
                        })
                });
                yield return new TestCaseData(210.732f, new Order()
                {
                    Price = 33.4f,
                    Items = new ReadOnlyCollection<Item>(
                        new List<Item>()
                        {
                            new Item() { Quantity = 20.4f, Product = new Product() { Price = 10.33f } }
                        })
                });
                yield return new TestCaseData(311f, new Order()
                {
                    Items = new ReadOnlyCollection<Item>(
                        new List<Item>()
                        {
                            new Item() { Quantity = 10, Product = new Product() { Price = 10.5f } },
                            new Item() { Quantity = 1, Product = new Product() { Price = 1f } },
                            new Item() { Quantity = 10, Product = new Product() { Price = 20.5f } }
                        })
                });
            }

        }

        [TestCaseSource("CountOrderForPriceCalculatingTestCases")]
        [Parallelizable(ParallelScope.All)]
        public void OrderOrderForPriceCalculatingTest(float result, Order order)
        {
            order.Recalculate();
            Assert.AreEqual(result, order.Price);
            Assert.Pass();
        }
    }
}