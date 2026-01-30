using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookstoreProjectData.Entities
{
    
    public class Order_Book
    {
        [ForeignKey(nameof(Book))]
        public Guid BookId { get; set; }
        public Book Book { get; set; } = null!;

        [ForeignKey(nameof(Order))]

        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
