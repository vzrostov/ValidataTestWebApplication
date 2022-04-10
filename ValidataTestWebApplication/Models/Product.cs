using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ValidataTestWebApplication.Models
{
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

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public float Price { get; set; }
    }
}