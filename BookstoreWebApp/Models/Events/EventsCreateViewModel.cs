using BookstoreProjectData.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookstoreWebApp.Models.Events
{
    public class EventsCreateViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = null!;

        [Required]
        public DateTime DateAndTime { get; set; }

        [ForeignKey(nameof(Author))]
        public Guid AuthorId { get; set; }
        public Author Author { get; set; } = null!;
    }
}
