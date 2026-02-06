using BookstoreProjectData.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookstoreWebApp.Models.Books
{
    public class BooksIndexViewModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = null!;

        public decimal Price { get; set; }

        public string CoverImageUrl { get; set; } = null!;

        public string AuthorName { get; set; } = null!;
        
        public decimal PromotionPercent { get; set; }
    }
}
