using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookstoreProjectData.Entities
{
    public class Publisher_Book
    {
        [ForeignKey(nameof(Publisher))]
        public Guid PublisherId { get; set; }
        public Publisher Publisher { get; set; } = null!;

        [ForeignKey(nameof(Book))]
        public Guid BookId { get; set; }
        public Book Book { get; set; } = null!;

        [Required]
        public string Language { get; set; } = null!;
    }
}
