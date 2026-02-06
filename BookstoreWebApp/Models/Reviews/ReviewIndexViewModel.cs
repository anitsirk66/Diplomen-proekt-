namespace BookstoreWebApp.Models.Reviews
{
    public class ReviewIndexViewModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = null!;
        public string Text { get; set; } = null!;
        public DateTime DateAndTime { get; set; }
        public Guid BookId { get; set; }
    }
}
