namespace backend.Models.DTOs
{
    public class AddBookDTO
    {
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Genre { get; set; }
        public float? Rating { get; set; }
        public string? Review { get; set; } 
    }
}