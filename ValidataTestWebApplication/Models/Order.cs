using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading;
using System.Web;

namespace ValidataTestWebApplication.Models
{
    public class Order
    {
        [Required]
        public int OrderId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public float Price { get; set; }

        [Required]
        public int CustomerID { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual ICollection<Item> Items { get; set; }

        public void Recalculate()
        {
            var newprice = Items?.Aggregate(0f, (total, item) => total + (item.Product?.Price ?? 0f) * item.Quantity);
            Price = newprice ?? 0f; // TODO after must be atomic?
        }
    }
}