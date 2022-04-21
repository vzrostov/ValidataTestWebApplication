using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ValidataTest.Core.Models
{
    /// <summary>
    /// Product is a dictionary of goods for Order
    /// </summary>
    public class Product
    {
        public Product() { }
        public Product(string name, float price) 
        { 
            Name = name;
            Price = price;
        }

        [Required]
        public int ProductId { get; set; }

        /// <summary>
        /// Provides product name
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        /// <summary>
        /// Provides price for one Unit
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        public float Price { get; set; }
    }
}