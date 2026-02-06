using BookstoreProjectData.Entities;
using System.ComponentModel.DataAnnotations;

namespace BookstoreWebApp.Models.Authors
{
    public class AuthorsIndexViewModel
    {
        public Guid Id { get; set; }

        public string FullName { get; set; } = null!;

        public string Biography { get; set; } = null!;

        public string Nationality { get; set; } = null!;

        public string ImageUrl { get; set; } = null!;
    }
}
