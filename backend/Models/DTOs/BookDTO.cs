namespace backend.Models.DTOs
{
    public class BookDTO
    {
        public string? Title { get; set; }
        public string? Author { get; set; }
        public int GenreId { get; set; }
        public int UserId { get; set; }
        public string? ReviewContent { get; set; }
        public int RatingScore { get; set; }

    }

    public class BookDTOResponse : BookDTO
    {
        public int BookId { get; set; }

        public string? Genre { get; set; }
    }
}