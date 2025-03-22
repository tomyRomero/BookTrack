namespace backend.Models.DTOs
{
    public class ReviewDTO
    {
        public int UserId { get; set; }
        public int BookId { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}