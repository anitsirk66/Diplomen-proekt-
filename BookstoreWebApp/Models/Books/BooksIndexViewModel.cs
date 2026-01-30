using BookstoreProjectData.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookstoreWebApp.Models.Books
{
    public class BooksIndexViewModel
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

        [ForeignKey(nameof(Author))]
        public Guid AuthorId { get; set; }
        public Author Author { get; set; } = null!;


        [ForeignKey(nameof(Promotion))]
        public Guid? PromotionId { get; set; }
        public Promotion? Promotion { get; set; }
    }
}
