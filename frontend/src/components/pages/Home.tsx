import { useNavigate} from "react-router-dom";

const Home = () => {

  const navigate = useNavigate();

  const handleLogin = () => {
    navigate('/login');
  };
  
  const handleSignup = () => {
    navigate('/signup');
  };

  return (
    <div className="flex justify-center items-center h-screen">
      <div className="border border-black text-center bg-white p-10 rounded-lg shadow-xl w-full max-w-md">
        <h1 className="text-4xl font-bold text-gray-800 mb-6">
          Welcome to BookTrack
        </h1>
        <p className="text-lg text-gray-600 mb-8">
          Where you can share with others your favorite books. Get started now by logging in or signing up!
        </p>
        
        <div className="flex flex-col gap-4">
          {/* Login Button */}
          <button
            onClick={handleLogin}
            className="bg-blue-600 text-white px-6 py-3 rounded-md text-xl font-semibold hover:bg-blue-700 transition duration-300"
          >
            Login
          </button>

          {/* Sign Up Button */}
          <button
            onClick={handleSignup}
            className="bg-teal-600 text-white px-6 py-3 rounded-md text-xl font-semibold hover:bg-teal-700 transition duration-300"
          >
            Sign Up
          </button>
        </div>
      </div>
    </div>
  );
};

export default Home;