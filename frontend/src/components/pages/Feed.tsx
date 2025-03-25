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

  const { state } = useAuth();
  const [activeTab, setActiveTab] = useState("myBooks");
  const [myBooks, setMyBooks] = useState<Book[]>([]);
  const [otherBooks, setOtherBooks] = useState<Book[]>([]);

  const navigate = useNavigate();

  const decodedToken = decodeToken(state.token);
  const username = decodedToken?.Username;

  useEffect(() => {
    fetchBooks();
  }, [activeTab]);

  const fetchBooks = async () => {
    try {
       // Fetch books from other users
       const otherBooksRes =  await fetch(`${import.meta.env.VITE_FETCH_NONUSER_BOOKS_URL}`, {
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

  const updateBook = async (bookId: number) => {
   navigate(`/update/${bookId}`);
  };

  const createBook = () => {
   navigate('/create');
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

      {/* Books list */}
      <div>
        {activeTab === "myBooks" ? (
          myBooks.length > 0 ? (
            <div className="space-y-4">
              {myBooks.map((book) => (
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
            <p>No books found.</p>
          )
        ) : (
          otherBooks.length > 0 ? (
            <div className="space-y-4">
              {otherBooks.map((book) => (
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
            <p>No books found.</p>
          )
        )}
      </div>
    </div>
  );
};

export default Feed;