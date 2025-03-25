import { useAuth } from '../Auth';
import { useNavigate} from "react-router-dom";

const Navbar = () => {
  const { state, dispatch } = useAuth(); 

  const navigate = useNavigate();

  const handleLogout = () => {
    dispatch({ type: "LOGOUT" }); 
    navigate("/");
  };

  return (
    <nav className="bg-white text-white p-4 flex justify-between items-center shadow-lg">
      {/* Logo on the left */}
      <div className="flex items-center cursor-pointer" onClick={() => navigate("/") }>
        <img
          src="/book-open-icon.png" 
          alt="Logo"
          className="w-10 h-10 mr-2" 
        />
        <span className="text-xl font-bold text-black">Book Track</span>
      </div>

      {/* Title on the right and conditional logout */}
      <div className="flex items-center">
        {state.isLoggedIn ? (
          <button
            onClick={handleLogout}
            className="bg-red-600 text-white px-4 py-2 rounded-md hover:bg-red-700 transition duration-300 cursor-pointer"
          >
            Logout
          </button>
        ) : (
          <button className="bg-green-600 text-white px-4 py-2 rounded-md hover:bg-green-700 transition duration-300 cursor-pointer"
          onClick={()=> navigate("/login")}
          >
            Login
          </button>
        )}
      </div>
    </nav>
  );
};

export default Navbar;