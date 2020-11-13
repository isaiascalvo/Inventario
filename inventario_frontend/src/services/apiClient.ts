import axios from "axios";

export const apiClient = axios.create({
  // baseURL: `http://192.168.1.111:8081/api`,
  // baseURL: `https://localhost:44386/api`,
  baseURL:
    process.env.NODE_ENV === "development"
      ? "https://localhost:44386/api"
      : "https://localhost:8081/api",
  withCredentials: false,
  headers: {
    Accept: "application/json",
    "Content-Type": "application/json"
  }
});
