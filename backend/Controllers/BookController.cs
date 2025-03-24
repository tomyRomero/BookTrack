
using System.Security.Claims;
using backend.Models.DTOs;
using backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        // Create a new book
        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] BookDTO bookDto)
        {
            try
            {
                 // Get the user ID from the JWT token
                var userId = User.FindFirstValue(ClaimTypes.Name);

                if (string.IsNullOrWhiteSpace(userId))
                {
                    return Unauthorized("User ID is not available in the token.");
                }

                // Assign the user ID from the token to the DTO
                bookDto.UserId = int.Parse(userId); 

                var createdBook = await _bookService.CreateBookAsync(bookDto);
                return CreatedAtAction(nameof(GetBookById), new { bookId = createdBook.BookId }, createdBook);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Get book by ID
        [HttpGet("{bookId}")]
        public async Task<IActionResult> GetBookById(int bookId)
        {
            var book = await _bookService.GetBookByIdAsync(bookId);
            if (book == null)
            {
                return NotFound(new { message = "Book not found." });
            }
            return Ok(book);
        }

        // Get all books
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await _bookService.GetAllBooksAsync();
            return Ok(books);
        }

        // Update book
        [HttpPut("{bookId}")]
        [Authorize]
        public async Task<IActionResult> UpdateBook(int bookId, [FromBody] BookDTO bookDto)
        {
            try
            {
                // Get the user ID from the JWT token
                var userId = User.FindFirstValue(ClaimTypes.Name);

                if (string.IsNullOrWhiteSpace(userId))
                {
                    return Unauthorized(new { message = "User ID is not available in the token." });
                }

                // Assign the user ID from the token to the DTO
                bookDto.UserId = int.Parse(userId); 

                var updatedBook = await _bookService.UpdateBookAsync(bookId, bookDto);
                if (updatedBook == null)
                {
                    return NotFound(new { message = "Book not found." });
                }
                return Ok(updatedBook);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Delete book
        [HttpDelete("{bookId}")]
        [Authorize]
        public async Task<IActionResult> DeleteBook(int bookId)
        {
            var deleted = await _bookService.DeleteBookAsync(bookId);
            if (!deleted)
            {
                return NotFound(new { message = "Book not found or could not be deleted." });
            }
            return NoContent();
        }

        // GET api/books/user/{userId}
        [HttpGet("user")]
        [Authorize]
        public async Task<ActionResult<List<BookDTOResponse>>> GetBooksByUser()
        {
            // Get the user ID from the JWT token
            var userIdClaim = User.FindFirstValue(ClaimTypes.Name);

             if (string.IsNullOrWhiteSpace(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "User ID is not available or invalid in the token." });
            }

            // Call the service method to get books by the user
            var books = await _bookService.GetBooksByUserAsync(userId);
            
            // If no books are found, return NotFound status
            if (books == null || books.Count == 0)
            {
                return NotFound(new { message = "No books found for this user." });
            }

            // Return the list of BookDTOResponse as a 200 OK response
            return Ok(books);
        }

        // GET api/books/notuser/{userId}
        [HttpGet("notuser")]
        [Authorize]
        public async Task<ActionResult<List<BookDTOResponse>>> GetBooksNotByUser()
        {
            // Get the user ID from the JWT token
            var userIdClaim = User.FindFirstValue(ClaimTypes.Name);

             if (string.IsNullOrWhiteSpace(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "User ID is not available or invalid in the token." });
            }

            // Call the service method to get books not by the user
            var books = await _bookService.GetBooksNotByUserAsync(userId);

            // If no books are found, return NotFound status
            if (books == null || books.Count == 0)
            {
                return NotFound(new { message = "No books found that do not belong to this user." });
            }

            

            // Return the list of BookDTOResponse as a 200 OK response
            return Ok(books);
        }

    }
}