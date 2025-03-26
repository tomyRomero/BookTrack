import { useState, useEffect } from "react";
import { useAuth } from "../../Auth";
import { toast } from "react-toastify";
import { useNavigate } from "react-router-dom";
import { decodeToken } from "../../utils";

const Feed = () => {
  interface Book {
    bookId: number;
    genre: string;
    username: string;
    title: string;
    author: string;
    genreId: number;
    userId: number;
    reviewContent: string;
    ratingScore: number;
  }

  interface Genre {
    genreId: number;
    name: string;
  }

  const { state } = useAuth();
  const [activeTab, setActiveTab] = useState("myBooks");
  const [myBooks, setMyBooks] = useState<Book[]>([]);
  const [otherBooks, setOtherBooks] = useState<Book[]>([]);
  const [genres, setGenres] = useState<Genre[]>([]);
  const [selectedGenre, setSelectedGenre] = useState<string | "">("");
  const [ratingSort, setRatingSort] = useState<string>("bestToWorst");

  const navigate = useNavigate();

  const decodedToken = decodeToken(state.token);
  const username = decodedToken?.Username;

  useEffect(() => {
    fetchBooks();
    fetchGenres();
  }, [activeTab, selectedGenre, ratingSort]);

  // Fetch books from the API
  const fetchBooks = async () => {
    try {
      // Fetch books from other users
      const otherBooksRes = await fetch(`${import.meta.env.VITE_FETCH_NONUSER_BOOKS_URL}`, {
        headers: {
          Authorization: `Bearer ${state.token}`,
        },
      });

      if (!otherBooksRes.ok) {
        throw new Error("Failed to fetch other users' books");
      }
      const otherBooksData = await otherBooksRes.json();
      setOtherBooks(otherBooksData);

      // Fetch user's books
      const userBooksRes = await fetch(`${import.meta.env.VITE_FETCH_USER_BOOKS_URL}`, {
        headers: {
          Authorization: `Bearer ${state.token}`,
        },
      });

      if (!userBooksRes.ok) {
        throw new Error(`Failed to fetch your books: ${userBooksRes.statusText}`);
      }
      const userBooksData = await userBooksRes.json();
      setMyBooks(userBooksData);
    } catch (error: Error | any) {
      console.error(error.message || "Failed to fetch books.");
    }
  };

  // Fetch genres from the API
  const fetchGenres = async () => {
    try {
      const genresRes = await fetch(`${import.meta.env.VITE_FETCH_GENRE_URL}`);
      const genresData = await genresRes.json();
      setGenres(genresData);
    } catch (error: Error | any) {
      console.error("Failed to fetch genres", error);
    }
  };

  // Delete a book
  const deleteBook = async (bookId: number) => {
    try {
      const isConfirmed = window.confirm("Are you sure you want to delete this book?");
      if (!isConfirmed) return;

      const deleteRes = await fetch(`${import.meta.env.VITE_DELETE_BOOK_URL}/${bookId}`, {
        method: "DELETE",
        headers: {
          Authorization: `Bearer ${state.token}`,
        },
      });

      if (!deleteRes.ok) {
        throw new Error("Failed to delete the book");
      }

      toast.success("Book deleted successfully!");

      // Update state after deletion
      setMyBooks((prevBooks) => prevBooks.filter((book) => book.bookId !== bookId));
      fetchBooks();
    } catch (error: Error | any) {
      toast.error(error.message || "Failed to delete book.");
    }
  };

  // Update a book
  const updateBook = async (bookId: number) => {
    navigate(`/update/${bookId}`);
  };

  // Create a new book
  const createBook = () => {
    navigate("/create");
  };

  // Handle sorting by rating
  const handleSort = (books: Book[]) => {
    if (ratingSort === "bestToWorst") {
      return books.sort((a, b) => b.ratingScore - a.ratingScore);
    } else {
      return books.sort((a, b) => a.ratingScore - b.ratingScore);
    }
  };

  // Filter books by genre
  const filteredBooks = (books: Book[]) => {
    return books.filter((book) => (selectedGenre ? book.genre === selectedGenre : true));
  };

  // Combine the genre filtering and sorting
  const getFilteredAndSortedBooks = (books: Book[]) => {
    const filtered = filteredBooks(books);
    return handleSort(filtered);
  };

  return (
    <div className="container mx-auto p-4">
      {username && (
        <div className="mb-6">
          <h2 className="text-2xl font-semibold">Welcome, {username}!</h2>
        </div>
      )}

      <div className="flex justify-between items-center mb-6">
        <h1 className="text-3xl font-bold">Books Feed</h1>
        <button
          onClick={createBook}
          className="bg-blue-600 text-white px-6 py-2 rounded-md hover:bg-blue-700 transition duration-300 cursor-pointer"
        >
          Create Book
        </button>
      </div>

      <div className="flex space-x-4 mb-4">
        <button
          className={`py-2 px-6 rounded-md ${activeTab === "myBooks" ? "bg-teal-500 text-white" : "bg-gray-200"} cursor-pointer`}
          onClick={() => setActiveTab("myBooks")}
        >
          My Books
        </button>
        <button
          className={`py-2 px-6 rounded-md ${activeTab === "otherBooks" ? "bg-teal-500 text-white" : "bg-gray-200"} cursor-pointer`}
          onClick={() => setActiveTab("otherBooks")}
        >
          Explore Other Books
        </button>
      </div>

      {/* Filters */}
      <div className="mb-4 flex space-x-4">
        <select
          value={selectedGenre}
          onChange={(e) => setSelectedGenre(e.target.value)}
          className="p-2 border rounded-md cursor-pointer"
        >
          <option value="">All Genres</option>
          {genres.map((genre) => (
            <option key={genre.genreId} value={genre.name}>
              {genre.name}
            </option>
          ))}
        </select>

        <select
          value={ratingSort}
          onChange={(e) => setRatingSort(e.target.value)}
          className="p-2 border rounded-md cursor-pointer"
        >
          <option value="bestToWorst">Best to Worst Rating</option>
          <option value="worstToBest">Worst to Best Rating</option>
        </select>
      </div>

      {/* Books list */}
      <div>
      {activeTab === "myBooks" ? (
          myBooks.length > 0 ? (
            getFilteredAndSortedBooks(myBooks).length > 0 ? (
              <div className="space-y-4">
                {getFilteredAndSortedBooks(myBooks).map((book) => (
                  <div key={book.bookId} className="border p-4 rounded-lg shadow-md bg-white">
                    <h3 className="text-xl font-semibold">{book.title}</h3>
                    <p className="text-gray-600">Author: {book.author}</p>
                    <p className="text-gray-600">Genre: {book.genre}</p>
                    <p className="text-gray-500">Review: {book.reviewContent}</p>
                    <p className="text-green-600 mt-2">Rating: <span className="font-semibold">{book.ratingScore} / 5</span></p>
                    <div className="mt-4">
                      <button
                        onClick={() => updateBook(book.bookId)}
                        className="bg-teal-600 text-white px-4 py-2 rounded-md hover:bg-teal-700 transition duration-300 mr-2 cursor-pointer"
                      >
                        Update
                      </button>
                      <button
                        onClick={() => deleteBook(book.bookId)}
                        className="bg-red-600 text-white px-4 py-2 rounded-md hover:bg-red-700 transition duration-300 cursor-pointer"
                      >
                        Delete
                      </button>
                    </div>
                  </div>
                ))}
              </div>
            ) : (
              <div className="text-center py-6 text-gray-500">
                <p>No books found in this genre.</p>
              </div>
            )
          ) : (
            <div className="text-center py-6 text-gray-500">
              <p>No books found. Start adding some!</p>
            </div>
          )
        ): (
          otherBooks.length > 0 ? (
            getFilteredAndSortedBooks(otherBooks).length > 0 ? (
              <div className="space-y-4">
                {getFilteredAndSortedBooks(otherBooks).map((book) => (
                  <div key={book.bookId} className="border p-6 rounded-lg shadow-lg bg-white hover:shadow-xl transition duration-300">
                    <div className="flex items-center space-x-3 mb-4">
                      <span className="font-semibold text-teal-600">{book.username}</span>
                      <span className="text-sm text-gray-500">submitted this book</span>
                    </div>
                    <h3 className="text-2xl font-semibold text-gray-800 mb-2">{book.title}</h3>
                    <p className="text-lg text-gray-700">Author: <span className="font-medium">{book.author}</span></p>
                    <p className="text-lg text-gray-700">Genre: <span className="font-medium">{book.genre}</span></p>
                    <p className="text-gray-600 mt-2">Review: <span className="italic">{book.reviewContent}</span></p>
                    <p className="text-green-600 mt-2">Rating: <span className="font-semibold">{book.ratingScore} / 5</span></p>
                  </div>
                ))}
              </div>
            ) : (
              <div className="text-center py-6 text-gray-500">
                <p>No books found in this genre.</p>
              </div>
            )
          ) : (
            <div className="text-center py-6 text-gray-500">
              <p>No books found. Explore some books from others!</p>
            </div>
          )
        )}
      </div>
    </div>
  );
};

export default Feed;