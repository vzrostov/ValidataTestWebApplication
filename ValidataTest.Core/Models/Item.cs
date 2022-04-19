using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ValidataTestWebApplication.Models
{
    /// <summary>
    /// Item is a part of Order, connecting Order and Product
    /// </summary>
    public class Item
    {
        public Item() { }
        public Item(float quantity, int productID, Product product, int orderID, Order order) 
        { 
            Quantity = quantity;
            ProductID = productID;
            Product = product;
            OrderID = orderID;
            Order = order;
        }

        [Required]
        public int ItemId { get; set; }

        /// <summary>
        /// Provides quantity of Product in some proper Units
        /// </summary>
        [Required]
        public float Quantity { get; set; }

        [Required]
        public int ProductID { get; set; }

        /// <summary>
        /// Provides Product. Product is a non-changeable in this context entity
        /// </summary>
        public virtual Product Product { get; set; }

        [Required]
        public int OrderID { get; set; }

        /// <summary>
        /// Provides Order
        /// </summary>
        public virtual Order Order { get; set; }
    }
}