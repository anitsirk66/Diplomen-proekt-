using BookstoreProjectData.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookstoreWebApp.Models.Books
{
    public class BooksEditViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; } = null!;

        [Required]
        [Range(typeof(decimal), "0.01", "2000")]
        public decimal Price { get; set; }

        [Required]
        [Url]
        public string CoverImageUrl { get; set; } = null!;

        [Required]
        [MinLength(30), MaxLength(300)]
        public string Synopsis { get; set; } = null!;


        [ForeignKey(nameof(Author))]
        public Guid AuthorId { get; set; }
        public Author Author { get; set; } = null!;


        [ForeignKey(nameof(Genre))]
        public Guid GenreId { get; set; }
        public Genre Genre { get; set; } = null!;


        [ForeignKey(nameof(Promotion))]
        public Guid? PromotionId { get; set; }
        public Promotion? Promotion { get; set; }

        public List<Publisher_Book> Publishers_Books { get; set; } = new List<Publisher_Book>();
    }
}
