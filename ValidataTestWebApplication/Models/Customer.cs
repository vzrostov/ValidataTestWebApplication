using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ValidataTestWebApplication.Models
{
    public class Customer
    {
        public Customer() {}
        public Customer(string firstName, string lastName, string address, string postalCode, ICollection<Order> orders) 
        { 
            FirstName = firstName;
            LastName = lastName;
            Address = address;
            PostalCode = postalCode;
            Orders = orders;
        }

        [Key]
        public int CustomerID { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [StringLength(200)]
        public string Address { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>https://www.quora.com/What-is-the-maximum-number-of-digits-in-a-zipcode-postal-code-for-a-place-in-this-universe</remarks>
        [Required]
        [DataType(DataType.PostalCode)]
        [StringLength(15)]
        public string PostalCode { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

    }
}