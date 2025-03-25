import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { toast } from 'react-toastify';
import { useAuth } from '../../Auth';

interface Genre {
  genreId: number;
  name: string;
}

const CreateBookPage = () => {
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
    fetch(`${import.meta.env.VITE_FETCH_GENRE_URL}`)
      .then((response) => response.json())
      .then((data) => setGenres(data))
      .catch((error) => console.error('Error fetching genres:', error));
  }, []);

  const handleCreateBook = async (e: React.FormEvent) => {
    e.preventDefault();

    const newBook = {
      title,
      author,
      genreId,
      reviewContent,
      ratingScore,
    };

    try {
      const response = await fetch(`${import.meta.env.VITE_CREATE_BOOK_URL}`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${state.token}`,
        },
        body: JSON.stringify(newBook),
      });

      if (response.ok) {
        toast.success('Book created successfully!');
        navigate('/feed');
      } else {
        toast.error(`Failed to create book! ${response.status}`);
      }
    } catch (error) {
      console.error('Error creating book:', error);
      toast.error('An error occurred while creating the book!');
    }
  };

  return (
    <div className="flex justify-center items-center h-screen">
      <div className="border border-black text-center bg-white p-10 rounded-lg shadow-xl w-full max-w-md">
        <h1 className="text-4xl font-bold text-gray-800 mb-6">Create Book</h1>

        {/* Back Button */}
        <button
          onClick={() => navigate('/feed')}
          className="text-teal-600 hover:underline mb-4 cursor-pointer"
        >
          &#8592; Back to Feed
        </button>

        <form onSubmit={handleCreateBook} className="space-y-4">
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

          {/* Genre Dropdown */}
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

          {/* Review Content */}
          <textarea
            placeholder="Review Content"
            value={reviewContent}
            onChange={(e) => setReviewContent(e.target.value)}
            className="w-full px-4 py-3 rounded-md border border-gray-300 focus:outline-none focus:ring-2 focus:ring-teal-500"
            rows={4}
          />

          {/* Rating Score */}
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
            Create Book
          </button>
        </form>
      </div>
    </div>
  );
};

export default CreateBookPage;