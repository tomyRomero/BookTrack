import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { toast } from 'react-toastify';
import { useAuth } from '../../Auth';

interface Genre {
  genreId: number;
  name: string;
}

const UpdateBookPage = () => {
  const { bookId } = useParams<{ bookId: string }>();
  const navigate = useNavigate();
  const { state } = useAuth();
  const [title, setTitle] = useState('');
  const [author, setAuthor] = useState('');
  const [genreId, setGenreId] = useState<number>(2); 
  const [reviewContent, setReviewContent] = useState('');
  const [ratingScore, setRatingScore] = useState(1);
  const [genres, setGenres] = useState<Genre[]>([]); 
  
  if(!state.token) {
    navigate('/login');
  }

  useEffect(() => {
    // Fetch genres
    fetch(`${import.meta.env.VITE_FETCH_GENRE_URL}`)
      .then((response) => response.json())
      .then((data) => setGenres(data))
      .catch((error) => console.error('Error fetching genres:', error));

    // Fetch book details by bookId
    fetch(`${import.meta.env.VITE_FETCH_BOOK_URL}/${bookId}`)
      .then((response) => response.json())
      .then((data) => {
        setTitle(data.title);
        setAuthor(data.author);
        setGenreId(data.genreId);
        setReviewContent(data.reviewContent);
        setRatingScore(data.ratingScore);
      })
      .catch((error) => {
        console.error('Error fetching book:', error);
        toast.error('Failed to fetch book data!');
      });
  }, [bookId]);

  const handleUpdateBook = async (e: React.FormEvent) => {
    e.preventDefault();

    const updatedBook = {
      title,
      author,
      genreId,
      reviewContent,
      ratingScore,
    };

    try {
      const response = await fetch(`${import.meta.env.VITE_UPDATE_BOOK_URL}/${bookId}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${state.token}`,
        },
        body: JSON.stringify(updatedBook),
      });

      if (response.ok) {
        toast.success('Book updated successfully!');
        navigate('/feed');
      } else {
        toast.error('Failed to update book!');
      }
    } catch (error) {
      console.error('Error updating book:', error);
      toast.error('An error occurred while updating the book!');
    }
  };

  return (
    <div className="flex justify-center items-center h-screen">
      <div className="border border-black text-center bg-white p-10 rounded-lg shadow-xl w-full max-w-md">
        <h1 className="text-4xl font-bold text-gray-800 mb-6">Update Book</h1>

        <button
          onClick={() => navigate('/feed')}
          className="text-teal-600 hover:underline mb-4 cursor-pointer"
        >
          &#8592; Back to Feed
        </button>

        <form onSubmit={handleUpdateBook} className="space-y-4">
          <input
            type="text"
            placeholder="Title"
            value={title}
            onChange={(e) => setTitle(e.target.value)}
            className="w-full px-4 py-3 rounded-md border border-gray-300 focus:outline-none focus:ring-2 focus:ring-teal-500"
            required
          />
          <input
            type="text"
            placeholder="Author"
            value={author}
            onChange={(e) => setAuthor(e.target.value)}
            className="w-full px-4 py-3 rounded-md border border-gray-300 focus:outline-none focus:ring-2 focus:ring-teal-500"
            required
          />

          <select
            value={genreId}
            onChange={(e) => setGenreId(Number(e.target.value))}
            className="w-full px-4 py-3 rounded-md border border-gray-300 focus:outline-none focus:ring-2 focus:ring-teal-500"
            required
          >
            {genres.map((genre) => (
              <option key={genre.genreId} value={genre.genreId}>
                {genre.name}
              </option>
            ))}
          </select>

          <textarea
            placeholder="Review Content"
            value={reviewContent}
            onChange={(e) => setReviewContent(e.target.value)}
            className="w-full px-4 py-3 rounded-md border border-gray-300 focus:outline-none focus:ring-2 focus:ring-teal-500"
            rows={4}
          />
          <input
            type="number"
            placeholder="Rating Score"
            value={ratingScore}
            onChange={(e) => setRatingScore(Number(e.target.value))}
            min="1"
            max="5"
            className="w-full px-4 py-3 rounded-md border border-gray-300 focus:outline-none focus:ring-2 focus:ring-teal-500"
          />
          <button
            type="submit"
            className="w-full bg-teal-600 text-white px-6 py-3 rounded-md text-xl font-semibold hover:bg-teal-700 transition duration-300 cursor-pointer"
          >
            Update Book
          </button>
        </form>
      </div>
    </div>
  );
};

export default UpdateBookPage;