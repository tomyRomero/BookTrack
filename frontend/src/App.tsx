import "./index.css";
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import Layout from './components/pages/Layout';
import Home from "./components/pages/Home";
import LoginPage from "./components/pages/Login";
import SignUpPage from "./components/pages/Signup";
import { AuthProvider } from "./Auth";
import { ToastContainer } from "react-toastify";
import Feed from "./components/pages/Feed";

function App() {
  return (
    <AuthProvider>
      <ToastContainer position="top-right" autoClose={3000} />
    <Router>
      <Routes>
        <Route element={<Layout />}>
          <Route path="/" element={<Home />} />
          <Route path="/login" element={<LoginPage />} />
          <Route path="/signup" element={<SignUpPage />} />
          <Route path="/feed" element={<Feed />} />
        </Route>
      </Routes>
    </Router>
    </AuthProvider>

  );
  
}

export default App
