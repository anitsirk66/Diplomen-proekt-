using BookstoreProjectData.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookstoreWebApp.Models.Events
{
    public class EventsIndexViewModel
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
