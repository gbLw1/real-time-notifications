import axios from "axios";

const options = {
  baseURL: "http://localhost:3000",
};

const api = axios.create(options);

export default api;
