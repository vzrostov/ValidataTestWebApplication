using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ValidataTestWebApplication.Models
{
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

        [Required]
        public float Quantity { get; set; }

        [Required]
        public int ProductID { get; set; }

        public virtual Product Product { get; set; }

        [Required]
        public int OrderID { get; set; }

        public virtual Order Order { get; set; }
    }
}