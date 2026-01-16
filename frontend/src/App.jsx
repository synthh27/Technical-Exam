import ProtectedRoute from './components/ProtectedRoute.jsx';
import { AuthProvider } from './context/AuthProvider.jsx';
import { BrowserRouter, Routes, Route } from 'react-router-dom';

import './App.css'
import Register from './pages/Register.jsx';
import Tasks from "./pages/Tasks.jsx"
import Login from './pages/Login.jsx';

function App() {
  return (
    <BrowserRouter>
      <AuthProvider>
        <Routes>
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
          <Route path="/tasks" element={
            <ProtectedRoute>
              <Tasks />
            </ProtectedRoute>
            }
          />
        </Routes>
      </AuthProvider>
    </BrowserRouter>
  );
}

export default App
