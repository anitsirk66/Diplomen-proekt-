using Microsoft.EntityFrameworkCore.Diagnostics;
using System.ComponentModel.DataAnnotations;

namespace BookstoreWebApp.Models.Authors
{
    public class AuthorsCreateViewModel
    {
        [Required]
        [StringLength(50)]
        public string FullName { get; set; } = null!;

        [Required]
        [StringLength(200)]
        public string Biography { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string Nationality { get; set; } = null!;

        public string ImageUrl { get; set; } = null!;
    }
}
