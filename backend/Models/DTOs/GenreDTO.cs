namespace backend.Models.DTOs
{
    public class GenreDTO
    {
        public string? Name { get; set; }
    }

    public class GenreResponseDTO
    {
        public int GenreId { get; set; }
        public string? Name { get; set; }
    }
}