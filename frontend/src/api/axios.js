import axios from 'axios';

const api = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL || "http://localhost:7144/api",
  headers: {
    "Content-Type": "application/json",
  },
  withCredentials: true, // optional, only if using cookies
});

// INTERCEPTORS
api.interceptors.request.use((config => {
  // GET TOKEN FROM LOCAL STORAGE
  const token = localStorage.getItem('token');

  // IF TOKEN EXISTS, ADD IT TO REQUEST HEADERS AUTOMATICALLY
  if (token) config.headers['Authorization'] = `Bearer ${token}`;
  return config;
}, (error) => { return Promise.reject(error);}));

export default api;
  