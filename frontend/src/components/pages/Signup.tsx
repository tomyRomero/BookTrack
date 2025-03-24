import { useState } from 'react';
import { useNavigate} from "react-router-dom";
import { toast } from "react-toastify";

const SignUpPage = () => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');

  const navigate = useNavigate();

  const handleSignUp = async (e: React.FormEvent) => {
    e.preventDefault();

    const payload = { username, password };

    try {
      const response = await fetch(`${import.meta.env.VITE_REGISTER_API_URL}`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(payload),
      });

      const data = await response.json();

      if (response.ok) {
        toast.success('Registration successful! Please log in.');
        navigate('/login'); 
      } else {
        toast.error(data.message || 'Registration failed.');
      }
    } catch (error) {
      toast.error('An error occurred. Please try again later.');
    }
  };

  return (
    <div className="flex justify-center items-center h-screen">
      <div className="border border-black text-center bg-white p-10 rounded-lg shadow-xl w-full max-w-md">
        <h1 className="text-4xl font-bold text-gray-800 mb-6">Sign Up</h1>

        <form onSubmit={handleSignUp} className="space-y-4">
          {/* Username */}
          <input
            type="text"
            placeholder="Username"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
            className="w-full px-4 py-3 rounded-md border border-gray-300 focus:outline-none focus:ring-2 focus:ring-teal-500"
            required
          />

          {/* Password */}
          <input
            type="password"
            placeholder="Password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            className="w-full px-4 py-3 rounded-md border border-gray-300 focus:outline-none focus:ring-2 focus:ring-teal-500"
            required
          />

          {/* Submit Button */}
          <button
            type="submit"
            className="w-full bg-teal-600 text-white px-6 py-3 rounded-md text-xl font-semibold hover:bg-teal-700 transition duration-300 cursor-pointer"
          >
            Sign Up
          </button>
        </form>

        <div className="flex items-center my-6">
          <div className="flex-grow border-t border-gray-300"></div>
          <span className="mx-4 text-gray-600">OR</span>
          <div className="flex-grow border-t border-gray-300"></div>
        </div>

        <p className="mt-6 text-gray-600">
          Already have an account?{' '}
          <span
            className="text-teal-600 cursor-pointer hover:underline" 
            onClick={() => navigate('/login')}
          >
            Login here
          </span>
        </p>
      </div>
    </div>
  );
};

export default SignUpPage;