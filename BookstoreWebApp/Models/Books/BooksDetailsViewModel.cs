using BookstoreProjectData.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookstoreWebApp.Models.Books
{
    public class BooksDetailsViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MinLength(30), MaxLength(300)]
        public string Synopsis { get; set; } = null!;

        [ForeignKey(nameof(Genre))]
        public Guid GenreId { get; set; }
        public Genre Genre { get; set; } = null!;


        public List<Order_Book> Orders_Books { get; set; } = new List<Order_Book>();
        public List<Publisher_Book> Publishers_Books { get; set; } = new List<Publisher_Book>();
        public List<Review> Reviews { get; set; } = new List<Review>();
    }
}
