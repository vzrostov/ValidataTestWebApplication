using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ValidataTestWebApplication.Models
{
    /// <summary>
    /// Customer is a person which makes Orders
    /// </summary>
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

        /// <summary>
        /// Provides Customer first name
        /// </summary>
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        /// <summary>
        /// Provides Customer last name
        /// </summary>
        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        /// <summary>
        /// Provides Customer address as a string
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Address { get; set; }

        /// <summary>
        /// Provides postal code of Customer address
        /// </summary>
        /// <remarks>https://www.quora.com/What-is-the-maximum-number-of-digits-in-a-zipcode-postal-code-for-a-place-in-this-universe</remarks>
        [Required]
        [DataType(DataType.PostalCode)]
        [StringLength(15)]
        public string PostalCode { get; set; }

        /// <summary>
        /// Provides Orders of Customer
        /// </summary>
        public virtual ICollection<Order> Orders { get; set; }

    }
}