using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading;
using System.Web;

namespace ValidataTestWebApplication.Models
{
    /// <summary>
    /// Order which was made by Customer, order contents are Items
    /// </summary>
    public class Order
    {
        public Order() { }
        public Order(DateTime date, float price, int customerID, Customer customer, ICollection<Item> items) 
        { 
            Date = date; 
            Price = price;
            CustomerID = customerID;
            Customer = customer;
            Items = items;
        }

        [Required]
        public int OrderId { get; set; }

        /// <summary>
        /// Provides date of Order
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        /// <summary>
        /// Provides total price of all Order Items
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        public float Price { get; set; }

        [Required]
        public int CustomerID { get; set; }

        /// <summary>
        /// Provides Order Customer
        /// </summary>
        public virtual Customer Customer { get; set; }

        /// <summary>
        /// Provides Order contents as Items
        /// </summary>
        public virtual ICollection<Item> Items { get; set; }

        /// <summary>
        /// Sum total price by all Items
        /// </summary>
        public void Recalculate()
        {
            var newprice = Items?.Aggregate(0f, (total, item) => total + (item.Product?.Price ?? 0f) * item.Quantity);
            Price = newprice ?? 0f; // must be atomic?
        }
    }
}