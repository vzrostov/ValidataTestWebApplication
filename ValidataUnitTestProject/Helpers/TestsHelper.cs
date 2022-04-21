using System;
using System.Collections.Generic;
using System.Linq;
using ValidataTest.Core.Models;

namespace ValidataUnitTests
{
    internal static class TestsHelper
    {
        internal static List<Customer> GetTestOrderedCustomers()
        {
            return new List<Customer>
            {
                new Customer("John", "Twains", "USA", "43990",
                    new List<Order>
                    {
                        new Order(new DateTime(2022, 04, 12, 2, 0, 0), customerID: 9, null, 
                            GetTestItems().Where(i => i.OrderID == 1).ToList()) { OrderId = 1 },
                        new Order(new DateTime(2022, 04, 12, 1, 0, 0), customerID: 9, null, null) { OrderId = 2 },
                        new Order(new DateTime(2022, 04, 12, 3, 0, 0), customerID: 9, null, null) { OrderId = 3 }
                    }
            ) { CustomerID = 9 }, // has 3 orders
                new Customer("Sara", "Parker", "UK", "444990", null) { CustomerID = 10 },
                new Customer("Mark", "Twain", "USA", "43990", null) { CustomerID = 19 } // has 1 order
            };
        }

        internal static List<Customer> GetTestCustomers()
        {
            return new List<Customer>
            {
                new Customer("John", "Twains", "USA", "43990", null) { CustomerID = 9 }, // has 3 orders
                new Customer("Sara", "Parker", "UK", "444990", null) { CustomerID = 10 },
                new Customer("Mark", "Twain", "USA", "43990", null) { CustomerID = 19 } // has 1 order
            };
        }

        internal static List<Customer> GetOneTestCustomers() => new List<Customer> { GetTestCustomers().First() };
        internal static List<Customer> GetOneTestOrderedCustomers() => new List<Customer> { GetTestOrderedCustomers().First() };

        internal static List<Order> GetTestOrders()
        {
            return new List<Order>
            {
                new Order(new DateTime(2022, 04, 12, 2, 0, 0), customerID: 9, null, null) { OrderId = 1 },
                new Order(new DateTime(2022, 04, 12, 1, 0, 0), customerID: 9, null, null) { OrderId = 2 },
                new Order(new DateTime(2022, 04, 12, 3, 0, 0), customerID: 9, null, null) { OrderId = 3 },
                new Order(new DateTime(2022, 04, 12, 1, 0, 0), customerID: 19, null, null) { OrderId = 4 }
            };
        }

        internal static List<Item> GetTestItems()
        {
            return new List<Item>
            {
                new Item(quantity: 1.5f, productID: 1, GetTestProducts().Where(p => p.ProductId == 1).First(), orderID: 1, null) { ItemId = 1 },
                new Item(quantity: 10f, productID: 2, GetTestProducts().Where(p => p.ProductId == 2).First(), orderID: 1, null) { ItemId = 2 },
                new Item(quantity: 1f, productID: 3, GetTestProducts().Where(p => p.ProductId == 3).First(), orderID: 1, null) { ItemId = 3 },
                new Item(quantity: 1f, productID: 3, GetTestProducts().Where(p => p.ProductId == 3).First(), orderID: 3, null) { ItemId = 4 }
            };
        }

        internal static List<Product> GetTestProducts()
        {
            return new List<Product>
            {
                new Product(name: "Product 1", price: 1f) { ProductId = 1 },
                new Product(name: "Product 2", price: 2f) { ProductId = 2 },
                new Product(name: "Product 3", price: 3f) { ProductId = 3 }
            };
        }
    }
}