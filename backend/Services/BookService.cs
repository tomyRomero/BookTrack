using backend.Models;
using backend.Models.DTOs;
using backend.Repositories;
using backend.Repositories.Interfaces;
using backend.Services.Interfaces;

namespace backend.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IUserRepository _userRepository; 
        private readonly IReviewRepository _reviewRepository;
        private readonly IRatingRepository _ratingRepository;

        public BookService(
            IBookRepository bookRepository, 
            IGenreRepository genreRepository,
            IUserRepository userRepository, 
            IReviewRepository reviewRepository,
            IRatingRepository ratingRepository)
        {
            _bookRepository = bookRepository;
            _genreRepository = genreRepository;
            _userRepository = userRepository; 
            _reviewRepository = reviewRepository;
            _ratingRepository = ratingRepository;
        }

        // Create a new book
        public async Task<BookDTOResponse> CreateBookAsync(BookDTO bookDto)
        {
            Console.WriteLine("Starting CreateBookAsync method...");

            // Validate Title and Author
            if (string.IsNullOrWhiteSpace(bookDto.Title))
            {
                Console.WriteLine("Validation failed: Title is missing.");
                throw new ArgumentException("Title is required.");
            }

            if (string.IsNullOrWhiteSpace(bookDto.Author))
            {
                Console.WriteLine("Validation failed: Author is missing.");
                throw new ArgumentException("Author is required.");
            }

            // Check if RatingScore is between 1 and 5
            if (bookDto.RatingScore < 1 || bookDto.RatingScore > 5)
            {
                Console.WriteLine($"Validation failed: Invalid rating score {bookDto.RatingScore}.");
                throw new Exception("Rating score must be between 1 and 5.");
            }

            // Check if ReviewContent is null or empty
            if (string.IsNullOrWhiteSpace(bookDto.ReviewContent))
            {
                Console.WriteLine("Validation failed: Review content is missing.");
                throw new Exception("Review content is required.");
            }

            Console.WriteLine($"Fetching genre with ID: {bookDto.GenreId}");
            var genre = await _genreRepository.GetGenreByIdAsync(bookDto.GenreId);
            if (genre == null)
            {
                Console.WriteLine("Error: Genre does not exist.");
                throw new Exception("Genre does not exist.");
            }

            Console.WriteLine($"Fetching user with ID: {bookDto.UserId}");
            var user = await _userRepository.GetUserByIdAsync(bookDto.UserId);
            if (user == null)
            {
                Console.WriteLine("Error: User does not exist.");
                throw new Exception("User does not exist.");
            }

            // Create a new transaction
            using (var transaction = await _bookRepository.BeginTransactionAsync()) 
            {
                try
                {
                    Console.WriteLine("Transaction started.");

                    // Create the book
                    var book = new Book
                    {
                        Title = bookDto.Title,
                        Author = bookDto.Author,
                        GenreId = bookDto.GenreId,
                        Genre = genre,
                        UserId = bookDto.UserId,
                        User = user
                    };

                    Console.WriteLine("Creating book in the repository...");
                    var createdBook = await _bookRepository.CreateBookAsync(book);
                    Console.WriteLine($"Book created successfully with ID: {createdBook.BookId}");

                    // Create and add the review and rating for the book
                    Console.WriteLine("Creating review and rating...");
                    await CreateReviewAndRatingAsync(createdBook.BookId, bookDto.UserId, bookDto.ReviewContent, bookDto.RatingScore);
                    Console.WriteLine("Review and rating created successfully.");

                     // Fetch the created review and rating
                    var review = await _reviewRepository.GetReviewByBookIdAsync(createdBook.BookId);
                    var rating = await _ratingRepository.GetRatingByBookIdAsync(createdBook.BookId);

                    // Commit the transaction after all operations
                    await transaction.CommitAsync();
                    Console.WriteLine("Transaction committed successfully.");

                      return new BookDTOResponse
                        {
                            BookId = book.BookId,
                            Title = book.Title,
                            Author = book.Author,
                            GenreId = book.GenreId,
                            Genre = book.Genre.Name,
                            UserId = book.UserId,
                            ReviewContent = review?.Content ?? "No review available",
                            RatingScore = rating?.Score ?? 0
                        };
                }
                catch (Exception ex)
                {
                    // Rollback the transaction if any error occurs
                    Console.WriteLine($"Error occurred: {ex.Message}. Rolling back transaction...");
                    await transaction.RollbackAsync();
                    throw new Exception("An unexpected error occurred while processing the transaction of CreateBookAsync", ex);
                }
            }
        }

        //Create Review and Rating
        private async Task CreateReviewAndRatingAsync(int bookId, int userId, string reviewContent, int ratingScore)
        {
            Console.WriteLine("Inside CreateReviewAndRatingAsync...");

            var review = new Review
            {
                BookId = bookId,
                UserId = userId,
                Content = reviewContent
            };

            var rating = new Rating
            {
                BookId = bookId,
                UserId = userId,
                Score = ratingScore
            };

            Console.WriteLine("Saving review and rating...");
                try
                {
                        await _reviewRepository.CreateReviewAsync(review);
                        await _ratingRepository.CreateRatingAsync(rating);
  
                }
                catch (Exception ex)
                {
                        throw new Exception("Error creating review and rating", ex);
                }
        }
        

        // Update review and rating for the book
        private async Task UpdateReviewAndRatingAsync(int bookId, int userId, string reviewContent, int ratingScore)
        {
           
                try
                {
                    // Fetch the existing review for this user and book
                    var existingReview = await _reviewRepository.GetReviewsByUserAsync(userId);
                    var review = existingReview.FirstOrDefault(r => r.BookId == bookId);

                    if (review == null)
                    {
                        // Create a new review if it doesn't exist
                        review = new Review
                        {
                            BookId = bookId,
                            UserId = userId,
                            Content = reviewContent
                        };
                        await _reviewRepository.CreateReviewAsync(review);
                    }
                    else
                    {
                        // Update existing review content
                        review.Content = reviewContent;
                        await _reviewRepository.UpdateReviewAsync(review);
                    }

                    // Fetch the existing rating for this user and book
                    var existingRating = await _ratingRepository.GetRatingsByUserAsync(userId);
                    var rating = existingRating.FirstOrDefault(r => r.BookId == bookId);

                    if (rating == null)
                    {
                        // Create a new rating if it doesn't exist
                        rating = new Rating
                        {
                            BookId = bookId,
                            UserId = userId,
                            Score = ratingScore
                        };
                        await _ratingRepository.CreateRatingAsync(rating);
                    }
                    else
                    {
                        // Update existing rating score
                        rating.Score = ratingScore;
                        await _ratingRepository.UpdateRatingAsync(rating);
                    }

                   
                }
                catch (Exception ex)
                {
                    throw new Exception("An unexpected error occurred while processing the transaction of UpdateReviewAndRating", ex);
                }
            }

    // Get a book by ID
    public async Task<Book?> GetBookByIdAsync(int bookId)
    {
            return await _bookRepository.GetBookByIdAsync(bookId);
    }

    // Get all books
    public async Task<List<Book>> GetAllBooksAsync()
    {
            return await _bookRepository.GetAllBooksAsync();
    }

    // Update an existing book with review and rating
    public async Task<BookDTOResponse?> UpdateBookAsync(int bookId, BookDTO bookDto)
    {
    // Validate inputs
    if (string.IsNullOrWhiteSpace(bookDto.Title))
    {
        throw new Exception("Title is required.");
    }

    if (string.IsNullOrWhiteSpace(bookDto.Author))
    {
        throw new Exception("Author is required.");
    }

    if (bookDto.RatingScore < 1 || bookDto.RatingScore > 5)
    {
        throw new Exception("Rating score must be between 1 and 5.");
    }

    if (string.IsNullOrWhiteSpace(bookDto.ReviewContent))
    {
        throw new Exception("Review content is required.");
    }

    // Check if the genre exists
    var genre = await _genreRepository.GetGenreByIdAsync(bookDto.GenreId) ?? throw new Exception("Genre does not exist.");

    // Check if the user exists
    var user = await _userRepository.GetUserByIdAsync(bookDto.UserId) ?? throw new Exception("User does not exist.");

    // Start a new transaction
    using (var transaction = await _bookRepository.BeginTransactionAsync())
    {
        try
        {
            // Fetch the existing book to update
            var book = await _bookRepository.GetBookByIdAsync(bookId) ?? throw new Exception("Book not found.");

            // Update book properties
            book.Title = bookDto.Title;
            book.Author = bookDto.Author;
            book.GenreId = bookDto.GenreId;
            book.Genre = genre;  
            book.UserId = bookDto.UserId;
            book.User = user;    

            // Update the book in the repository
            var updatedBook = await _bookRepository.UpdateBookAsync(book) ?? throw new Exception("Failed to update book.");

            // Update or create the review and rating
            await UpdateReviewAndRatingAsync(updatedBook.BookId, bookDto.UserId, bookDto.ReviewContent, bookDto.RatingScore);

            // Fetch the updated review and rating
            var review = await _reviewRepository.GetReviewByBookIdAsync(updatedBook.BookId);
            var rating = await _ratingRepository.GetRatingByBookIdAsync(updatedBook.BookId);

            // Commit the transaction after all operations
            await transaction.CommitAsync();

            return new BookDTOResponse
            {
                            BookId = book.BookId,
                            Title = book.Title,
                            Author = book.Author,
                            GenreId = book.GenreId,
                            Genre = book.Genre.Name,
                            UserId = book.UserId,
                            ReviewContent = review?.Content ?? "No review available",
                            RatingScore = rating?.Score ?? 0
            };
        }
        catch (Exception ex)
                {
                    // Rollback the transaction if any error occurs
                    await transaction.RollbackAsync();
                    throw new Exception("An unexpected error occurred while processing the transaction of UpdateBook", ex);
                }
        }
    }

        // Delete a book
    public async Task<bool> DeleteBookAsync(int bookId)
    {
            return await _bookRepository.DeleteBookAsync(bookId);
    }

    public async Task<List<BookDTOResponse>> GetBooksByUserAsync(int userId)
    {
        var books = await _bookRepository.GetBooksByUserIdAsync(userId);

        // Map the books to BookDTOResponse
        var bookDTOs = books.Select(b => new BookDTOResponse
        {
            BookId = b.BookId,
            Title = b.Title,
            Author = b.Author,
            GenreId = b.GenreId,
            UserId = b.UserId,
            Genre = b.Genre.Name, 
            ReviewContent = b.Reviews?.FirstOrDefault()?.Content ?? "No review available", 
            RatingScore = b.Ratings?.FirstOrDefault()?.Score ?? 0 
        }).ToList();

        return bookDTOs;
    }

    public async Task<List<BookDTOResponse>> GetBooksNotByUserAsync(int userId)
    {
        var books = await _bookRepository.GetBooksNotByUserIdAsync(userId);

        var bookDTOs = books.Select(b => new BookDTOResponse
        {
            BookId = b.BookId,
            Title = b.Title,
            Author = b.Author,
            GenreId = b.GenreId,
            UserId = b.UserId,
            Genre = b.Genre.Name, 
            ReviewContent = b.Reviews?.FirstOrDefault()?.Content ?? "No review available", 
            RatingScore = b.Ratings?.FirstOrDefault()?.Score ?? 0 
        }).ToList();

        return bookDTOs;
    }


    }
}