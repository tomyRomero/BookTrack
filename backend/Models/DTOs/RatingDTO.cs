namespace backend.Models.DTOs
{
    public class RatingDTO
    {
        public int RatingId { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public int Score { get; set; } 
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}