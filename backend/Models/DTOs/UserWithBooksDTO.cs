
namespace backend.Models.DTOs
{
    public class UserWithBooksDTO
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public List<BookDTO> Books { get; set; } = new();
    }
}