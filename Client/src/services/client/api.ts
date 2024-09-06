import axios from "axios";

const options = {
  baseURL: import.meta.env.VITE_API_URL,
};

const api = axios.create(options);

export default api;
