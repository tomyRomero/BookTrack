import { useState, useEffect } from "react";
import { useAuth } from "../../Auth"; 
import { toast } from "react-toastify";

const Feed = () => {
  const { state } = useAuth(); // Access global state for login info
  const [activeTab, setActiveTab] = useState("myBooks");
  const [myBooks, setMyBooks] = useState<any[]>([]);
  const [otherBooks, setOtherBooks] = useState<any[]>([]);

  useEffect(() => {
    fetchBooks();
  }, [activeTab]);

  const fetchBooks = async () => {
    try {
      // Fetch user's books
      const userBooksRes = await fetch(`${import.meta.env.VITE_FETCH_USER_BOOKS_URL}`, {
        headers: {
          Authorization: `Bearer ${state.token}`, 
        },
      });

      if (!userBooksRes.ok) {
        throw new Error("Failed to fetch your books");
      }
      const userBooksData = await userBooksRes.json();
      setMyBooks(userBooksData);

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
    } catch (error: Error | any) {
      toast.error(error.message || "Failed to fetch books.");
    }
  };

  const deleteBook = async (bookId: number) => {
    try {
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
      fetchBooks(); // Re-fetch books after deletion
    } catch (error: Error | any) {
      toast.error(error.message || "Failed to delete book.");
    }
  };

  const updateBook = async (bookId: number) => {
    // Open a modal or redirect to an update page to edit the book details
    toast.info("Redirect to update book page.");
  };

  const createBook = () => {
    // Redirect to a "Create Book" page
    toast.info("Redirect to create book page.");
  };

  return (
    <div className="container mx-auto p-4">
      {/* Header with Create Book button */}
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-3xl font-bold">Books Feed</h1>
        <button
          onClick={createBook}
          className="bg-blue-600 text-white px-6 py-2 rounded-md hover:bg-blue-700 transition duration-300"
        >
          Create Book
        </button>
      </div>

      {/* Tab navigation */}
      <div className="flex space-x-4 mb-4">
        <button
          className={`py-2 px-6 rounded-md ${activeTab === "myBooks" ? "bg-teal-500 text-white" : "bg-gray-200"}`}
          onClick={() => setActiveTab("myBooks")}
        >
          My Books
        </button>
        <button
          className={`py-2 px-6 rounded-md ${activeTab === "otherBooks" ? "bg-teal-500 text-white" : "bg-gray-200"}`}
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
                  <p className="text-yellow-500">Rating: {book.ratingScore}</p>
                  <div className="mt-4">
                    <button
                      onClick={() => updateBook(book.bookId)}
                      className="bg-teal-600 text-white px-4 py-2 rounded-md hover:bg-teal-700 transition duration-300 mr-2"
                    >
                      Update
                    </button>
                    <button
                      onClick={() => deleteBook(book.bookId)}
                      className="bg-red-600 text-white px-4 py-2 rounded-md hover:bg-red-700 transition duration-300"
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
                <div key={book.bookId} className="border p-4 rounded-lg shadow-md bg-white">
                  <h3 className="text-xl font-semibold">{book.title}</h3>
                  <p className="text-gray-600">Author: {book.author}</p>
                  <p className="text-gray-600">Genre: {book.genre}</p>
                  <p className="text-gray-500">Review: {book.reviewContent}</p>
                  <p className="text-yellow-500">Rating: {book.ratingScore}</p>
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