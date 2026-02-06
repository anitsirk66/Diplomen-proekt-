using BookstoreProjectData.Entities;
using BookstoreWebApp.Models.Reviews;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookstoreWebApp.Models.Books
{
    public class BooksDetailsViewModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = null!;

        public string Synopsis { get; set; } = null!;

        public string GenreName { get; set; } = null!;

        public decimal Price { get; set; }

        public string CoverImageUrl { get; set; } = null!;

        public string AuthorName { get; set; } = null!;
        public decimal PromotionPercent { get; set; }

        public List<ReviewIndexViewModel> Reviews { get; set; } = new List<ReviewIndexViewModel>();
    }
}
