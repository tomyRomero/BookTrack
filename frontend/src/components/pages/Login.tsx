import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { toast } from 'react-toastify'; 
import { useAuth } from '../../Auth';

const LoginPage = () => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const { dispatch } = useAuth(); 
  const navigate = useNavigate();

  const handleLogin = async (e: React.FormEvent) => {
    e.preventDefault();

    const payload = { username, password };

    try {
      const response = await fetch(`${import.meta.env.VITE_LOGIN_API_URL}`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(payload),
      });

      if (!response.ok) {
        throw new Error('Login failed. Please check your username and password.');
      }

      const data = await response.json();

      if (data.token) {

        dispatch({ type: 'LOGIN', payload: data.token });

        toast.success('Login successful!');
        navigate('/feed');
      } else {
        throw new Error('No token received.');
      }
    } catch (error: any) {
      toast.error(error.message || 'An error occurred. Please try again.');
    }
  };

  return (
    <div className="flex justify-center items-center h-screen">
      <div className="border border-black text-center bg-white p-10 rounded-lg shadow-xl w-full max-w-md">
        <h1 className="text-4xl font-bold text-gray-800 mb-6">Login</h1>

        <form onSubmit={handleLogin} className="space-y-4">
          <input
            type="text"
            placeholder="Username"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
            className="w-full px-4 py-3 rounded-md border border-gray-300 focus:outline-none focus:ring-2 focus:ring-teal-500"
            required
          />

          <input
            type="password"
            placeholder="Password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            className="w-full px-4 py-3 rounded-md border border-gray-300 focus:outline-none focus:ring-2 focus:ring-teal-500"
            required
          />

          <button
            type="submit"
            className="w-full bg-teal-600 text-white px-6 py-3 rounded-md text-xl font-semibold hover:bg-teal-700 transition duration-300 cursor-pointer"
          >
            Login
          </button>
        </form>

        <div className="flex items-center my-6">
          <div className="flex-grow border-t border-gray-300"></div>
          <span className="mx-4 text-gray-600">OR</span>
          <div className="flex-grow border-t border-gray-300"></div>
        </div>

        <p className="mt-6 text-gray-600">
          Don't have an account?{' '}
          <span
            className="text-teal-600 cursor-pointer hover:underline"
            onClick={() => navigate('/signup')}
          >
            Sign up here
          </span>
        </p>
      </div>
    </div>
  );
};

export default LoginPage;