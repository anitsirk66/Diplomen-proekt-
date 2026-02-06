using BookstoreProjectData.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookstoreWebApp.Models.Books
{
    public class BooksEditViewModel
    {
        [Required]
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


        [Required]
        public Guid AuthorId { get; set; }

        [Required]
        public Guid GenreId { get; set; }
        
        public Guid? PromotionId { get; set; }

    }
}
