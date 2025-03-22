using backend.Data_Access;
using backend.Models;
using backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace backend.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _context;

        public BookRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Create a new book
        public async Task<Book> CreateBookAsync(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return book;
        }

        // Get a book by ID
        public async Task<Book?> GetBookByIdAsync(int bookId)
        {
            return await _context.Books
                .Include(b => b.Genre)  // Include Genre data if needed
                .Include(b => b.User)   // Include User data if needed
                .FirstOrDefaultAsync(b => b.BookId == bookId);
        }

        // Get all books
        public async Task<List<Book>> GetAllBooksAsync()
        {
            return await _context.Books
                .Include(b => b.Genre)  // Include Genre data if needed
                .Include(b => b.User)   // Include User data if needed
                .ToListAsync();
        }

        // Update an existing book
        public async Task<Book?> UpdateBookAsync(Book book)
        {
            var existingBook = await _context.Books.FindAsync(book.BookId);
            if (existingBook == null)
            {
                return null;
            }

            existingBook.Title = book.Title;
            existingBook.Author = book.Author;
            existingBook.GenreId = book.GenreId;

            await _context.SaveChangesAsync();
            return existingBook;
        }

        // Delete a book
        public async Task<bool> DeleteBookAsync(int bookId)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null)
            {
                return false;
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return true;
        }

        // Start a new transaction
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

    }
}